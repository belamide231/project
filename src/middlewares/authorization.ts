import { Request, Response, NextFunction } from "express";
import { verifyToken } from "../utilities/jwt";
import 'express-session';

export const isAuthorized = (req: Request, res: Response, next: NextFunction) => {

    const payload = verifyToken(req.cookies.token);

    if(!payload.token) {

        res.cookie('token', '', {
            httpOnly: true,
            secure: false,
            path: '/',
            expires: new Date(0)
        });

        return res.redirect('/login');

    } else {

        if(JSON.parse(JSON.stringify(payload.payload)).role !== 'admin') {

            res.cookie('token', '', {
                httpOnly: true,
                secure: false,
                path: '/',
                expires: new Date(0)
            });

            return res.redirect('/login');
        }
    }

    const data = JSON.parse(JSON.stringify(payload.payload));

    req.session.user = {
        user: data.user,
        role: data.role,
    };

    console.log(req.session);
      

    next();
}