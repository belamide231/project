import { Router } from "express";
import { isAuthenticated } from "../middlewares/authentication";
import { isAuthorized } from "../middlewares/authorization";
import path from 'path';
import { verifyToken } from "../utilities/jwt";

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

    const payload = verifyToken(req.cookies.token);

    if(payload.token) {

        if(JSON.parse(JSON.stringify(payload.payload)).role === 'admin') {

            return res.redirect('/');
        }
    }

    res.cookie('token', '', {
        httpOnly: true,
        secure: false,
        path: '/',
        expires: new Date(0)
    });

    res.status(200).sendFile(path.join(__dirname, '../../public/browser/index.html'));
});

homeController.get('/', isAuthorized, (req, res) => {
    res.status(200).sendFile(path.join(__dirname, '../../public/browser/index.html'));
});

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