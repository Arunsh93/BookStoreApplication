Use BookStore;

Alter Procedure Sp_ResetPassword
(
	@EmailId VARCHAR(50),
	@NewPassword VARCHAR(20)
)
As
Begin
	If Exists (Select * From Users Where EmailId = @EmailId)
	begin
		Update [Users] Set [Password] = @NewPassword Where EmailId = @EmailId
	end
	Else
	begin
		Select 1;
	end	
End