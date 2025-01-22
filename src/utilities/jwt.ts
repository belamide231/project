import jwt from 'jsonwebtoken';
import dotenv from 'dotenv';
dotenv.config();

export const tokenizer = (id: number, role: string) => {

    const secret = process.env.JWT_PASSWORD_SECRET;
    return secret ? jwt.sign({ id, role }, secret, { expiresIn: '1y' }) : false;
}

export const verifyToken = (token: string) => {

    try {

        const secret = process.env.JWT_SECRET;

        if(secret) {

            const decode = jwt.verify(token, secret);
    
            return {
                token: false,
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