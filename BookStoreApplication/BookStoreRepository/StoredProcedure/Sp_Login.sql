USE [BookStore]

Create Procedure Sp_Login
(
	@EmailId VARCHAR(50),
	@Password VARCHAR(50)
)
AS
Begin
	Select * From Users Where EmailId = @EmailId And Password = @Password
End