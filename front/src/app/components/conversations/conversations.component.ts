import { Component, NgModule, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from '../../services/chat.service'; // Import the ChatService
import { CommonModule } from '@angular/common';
import { Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-conversations',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  templateUrl: './conversations.component.html',
  styleUrls: ['./conversations.component.css'],
})
export class ConversationComponent implements OnInit {
  messages: { sender: string; message: string; time: string; type: 'sent' | 'received'; priority?: boolean; status?: 'open' | 'pending' | 'closed'; ipLocation: string }[] = [];
  newMessage: string = '';
  selectedCategory: string = 'all';  // Default category
  selectedChat: { sender: string; message: string; time: string;  type: 'sent' | 'received'; priority?: boolean; status?: 'open' | 'pending' | 'closed'; ipLocation: string } | null = null;
  showOptions: boolean = false;
  loading: boolean = false;  // To track loading state
  composeModal: boolean = false;
  attachmentModal: boolean = false;

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.fetchMessagesByCategory();
    const themeColors = localStorage.getItem('themeColors');
    if (themeColors) {
      const colors = JSON.parse(themeColors);
      document.documentElement.style.setProperty('--primary-color', colors.primary);
      document.documentElement.style.setProperty('--secondary-color', colors.secondary);
      document.documentElement.style.setProperty('--accent-color', colors.accent);
      document.documentElement.style.setProperty('--whites-color', colors.whites);
    }
  }

  fetchMessagesByCategory(): void {
    this.loading = true;
    this.messages = this.chatService.getMessagesByType(this.selectedCategory);
    if (this.messages.length > 0) {
      this.selectedChat = { 
        sender: this.messages[0].sender, 
        message: this.messages[0].message, 
        time: this.messages[0].time, 
        priority: this.messages[0].priority, 
        status: this.messages[0].status, 
        ipLocation: this.messages[0].ipLocation ,
        type: this.messages[0].type
      }; // Show the first message as selected
    }
    this.loading = false;
  }

  changeCategory(category: string): void {
    this.selectedCategory = category;
    this.fetchMessagesByCategory();
  }

  sendMessage(): void {
    if (this.newMessage.trim()) {
      this.chatService.sendMessage(this.newMessage, this.selectedCategory);
      this.fetchMessagesByCategory();
      this.newMessage = '';  // Clear the input field
    }
  }

  editMessage(index: number): void {
    const newMessage = prompt('Edit message:', this.messages[index].message);
    if (newMessage !== null) {
      this.chatService.editMessage(index, newMessage, this.selectedCategory);
      this.fetchMessagesByCategory();
    }
  }



  viewDetails(index: number): void {
    const message = this.messages[index];
    alert(`Message details:\nSender: ${message.sender}\nTime: ${message.time}\nMessage: ${message.message}\nLocation: ${message.ipLocation}`);
  }

  getLatestMessageTime(category: string): string {
    return this.chatService.getLatestTime(category);
  }

  // New function to handle when a user clicks a message
  selectMessage(message: { sender: string; message: string; time: string; type: 'sent' | 'received'; priority?: boolean; status?: 'open' | 'pending' | 'closed'; ipLocation: string }) {
    this.selectedChat = message;
  }
  addNotes(){
    alert('Notes added')
  }
  viewTranscript(){
    alert('Transcript viewed')
  }
  deleteMessage(){
    alert('Message deleted')
  }
 

  toggleOptions(): void {
    this.showOptions = !this.showOptions;
  }
  showComposeModal(){
    this.composeModal = !this.composeModal;
  }
  closeComposeModal(){
    this.composeModal = false;
  }
  sendComposeMessage(){
    alert('Message sent')
  }
  sendAttachment(){
    this.attachmentModal = !this.attachmentModal;
  }
  uploadFiles(){
    alert('Files uploaded')
  }



}

