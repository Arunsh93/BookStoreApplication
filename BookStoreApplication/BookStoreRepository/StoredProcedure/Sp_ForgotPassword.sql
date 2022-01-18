USE [BookStore]

Alter Procedure Sp_ForgotPassword
(
	@EmailId VARCHAR(50)
)
As
Begin
	If Exists(Select * from Users Where EmailId = @EmailId)
	begin
		Select 1
	end
	Else
	begin
		Select 0
	end
End