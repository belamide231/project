import jwt from 'jsonwebtoken';
import dotenv from 'dotenv';
dotenv.config();

export const tokenizer = (user: string, role: string) => {

    const secret = process.env.PASSWRD_JWT_SECRET;
    return secret ? jwt.sign({ user, role }, secret, { expiresIn: '1y' }) : false;
}

export const verifyToken = (token: string) => {

    try {

        const secret = process.env.PASSWRD_JWT_SECRET;

        if(secret) {

            const decode = jwt.verify(token, secret);
    
            return {
                token: true,
                payload: decode
            }
    
        } else {

            console.log("INVALID KEY");

            return {
                token: false,
                payload: null
            }
        }


    } catch {

        return {
            token: false,
            payload: null
        };
    }
}