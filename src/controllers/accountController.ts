import { Router, Request, Response, json } from "express";
import { loginAccountDTO } from "../dto/accountController/loginAccount";
import { tokenizer } from '../utilities/jwt';
import { createAccountDTO } from "../dto/accountController/createAccount";
import { createAccountService } from "../services/accountServices";


const accountController = Router();
const credentials = ['ibcadmin', 'gisadmin', 'jetadmin'];
const users: any = {
    ibcadmin: 'IBC admin',
    gisadmin: 'Gis admin',
    jetadmin: 'Jet admin'
};

accountController.post('/addAccountManager', (req: Request, res: Response) => {
    res.sendStatus(200);
});

accountController.post('/loginAccount', (req: Request, res: Response) => {
    const account = req.body as loginAccountDTO;

    if (credentials.includes(account.username) && credentials.includes(account.password) && account.username === account.password) {

        res.cookie('token', tokenizer(users[account.username], 'admin'), {
            httpOnly: true,
            secure: false,
            path: '/'
        });

        res.status(200).json({ message: 'Login successful' });

    } else {

        res.status(403).json({ message: 'Invalid credentials' });
    }
});

accountController.post('/logoutAccount', (req: Request, res: Response) => {

    res.cookie('token', '', {
        httpOnly: true, 
        secure: false,
        path: '/', 
        expires: new Date(0)
    });

    res.status(200).json({ message: 'Cookie cleared, logged out successfully' });
    
});

accountController.post('/createAccount', async (req: Request, res: Response) => {
    const status = await createAccountService(req.body as createAccountDTO);
    res.sendStatus(status);
});

accountController.post('/deleteAccount', (req: Request, res: Response) => {

});

export default accountController;