
export class UserLogin
{
    public Email:string;
    public PasswordHash:string;
}

export class User extends UserLogin
{
    public Id:string;
    public FullName:string;
    public UserName: string;
    public IsAdmin: boolean;
    public IsAccountant: boolean;
    public IsEncoder: boolean;
    public ChangePasswordOnFirstLogin:boolean
}