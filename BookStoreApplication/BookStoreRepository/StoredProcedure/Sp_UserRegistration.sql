Create Database BookStore;

Use BookStore;

Create table Users
(
	UserId INT PRIMARY KEY IDENTITY (1, 1),
	FullName VARCHAR(50),
	EmailId VARCHAR(50),
	Password VARCHAR(20),
	PhoneNumber VARCHAR(15)
);

Select * from Users;

Alter Procedure Sp_Registration
(
	@FullName VARCHAR(50),
	@EmailId  VARCHAR(50),
	@Password VARCHAR(20),
	@PhoneNumber VARCHAR(15)
)
AS
Begin
	If(Select UserId From Users Where EmailId = @EmailId) is Not Null
	begin
		  Insert Into [Users](FullName, EmailId, Password, PhoneNumber)
		  Values(@FullName, @EmailId, @Password, @PhoneNumber)
	end
	Else
	begin
			Select 1;
	end
End