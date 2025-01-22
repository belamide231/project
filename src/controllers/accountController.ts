import { Router, Request, Response, json } from "express";
import { loginAccountDTO } from "../dto/accountController/loginAccount";
import { tokenizer } from '../utilities/jwt';
import { createAccountDTO } from "../dto/accountController/createAccount";
import { createAccountService } from "../services/accountServices";
import cookieParser from "cookie-parser";
import cors from 'cors';


const accountController = Router();
accountController.use(cookieParser());
accountController.use(json());
accountController.use(cors({ origin: 'http://localhost:4200', credentials: true }));

accountController.post('/addAccountManager', (req: Request, res: Response) => {
    res.sendStatus(200);
});

accountController.post('/loginAccount', (req: Request, res: Response) => {

    const account = req.body as loginAccountDTO;

    if(account.username === 'ibcadmin' && account.password === 'ibcadmin') {

        res.cookie('token', tokenizer(0, 'admin'));
        res.sendStatus(200);
        return;
    } else {


        res.sendStatus(403);
        return;
    }

});

accountController.post('/createAccount', async (req: Request, res: Response) => {
    const status = await createAccountService(req.body as createAccountDTO);
    res.sendStatus(status);
});

accountController.post('/deleteAccount', (req: Request, res: Response) => {

});

export default accountController;