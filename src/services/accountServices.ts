import { createAccountDTO } from "../dto/accountController/createAccount";
import { mysql } from "../app";

export const createAccountService = async (data: createAccountDTO) => {

    try {

        await mysql.promise().query(" ", [data.username, data.password]);

        return 200;

    } catch (error) {

        console.error(error);
        return 500;
    }
}