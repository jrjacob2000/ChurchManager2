
export class UserLogin
{
    public Email:string;
    public PasswordHash:string;
}

export class User extends UserLogin
{
    public FullName:string;
    public UserName: string;
    public ChangePasswordOnFirstLogin:boolean
}

