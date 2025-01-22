import { Router } from "express";
import { isAuthenticated } from "../middlewares/authentication";
import { isAuthorized } from "../middlewares/authorization";
import path from 'path';

const homeController = Router();

/*
''
'chat'
'users'
'notification'
'settings'
'profile'
'login'
*/

homeController.get('/login', (req, res) => {
    res.status(200).sendFile(path.join(__dirname, '../../public/browser/index.html'));
});

//homeController.get('/', (req, res) => {

//})

//homeController.get('/chat', isAuthenticated, isAuthorized, (req, res) => {
//    res.redirect('chat');
//});

//homeController.get('/users', isAuthenticated, isAuthorized, (req, res) => {
//    res.redirect('users');
//});

//homeController.get('/notification', isAuthenticated, isAuthorized, (req, res) => {
//    res.redirect('notification');
//});

//homeController.get('/settings', isAuthenticated, isAuthorized, (req, res) => {
//    res.redirect('settings');
//});

//homeController.get('/profile', isAuthenticated, isAuthorized, (req, res) => {
//    console.log("trigger");
//    res.redirect('profile');
//});

export default homeController;