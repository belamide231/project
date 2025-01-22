import { Request, Response, NextFunction } from "express";
import { verifyToken } from "../utilities/jwt";

export const isAuthenticated = (req: Request, res: Response, next: NextFunction) => {

    next();
}