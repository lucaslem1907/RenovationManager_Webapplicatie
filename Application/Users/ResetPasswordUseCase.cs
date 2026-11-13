using Application.Exceptions;
using Application.Interfaces;
using Resend;
using FluentResults;

namespace Application.Users
{
    public class ResetPasswordUseCase
    {
        private IUserRepository _repo;
        private IAuthService _authService;
        private readonly IResend _resend;
        private string resetPage = "http://localhost:3000/reset-password";
        private string fromEmail = "Acme <onboarding@resend.dev>";
        private string toEmail = "delivered@resend.dev";

        public ResetPasswordUseCase(IUserRepository repo, IAuthService authService, IResend resend)
        {
            _repo = repo;
            _authService = authService;
            _resend = resend;
        }

        //sends an email to the user with a link to reset their password
        public async Task<Result> SendEmail(string email)
        {
            if (email == null)
            {
                return Result.Fail(new OwnerNotFoundError("User not found"));
            }
            var user = await _repo.GetByEmail(email);
            if (user == null)
            {
                return Result.Fail(new OwnerNotFoundError("User not found"));
            }


            // Make message
            var message = new EmailMessage();
            message.From = fromEmail;
            message.To.Add(toEmail);
            message.Subject = "Password Reset for your Renovation Manager account";
            message.HtmlBody = "<h1>Your password reset request</h1>" +
                "<p>Please click the link below to reset your password.</p>" +
                $"<a href='{resetPage}?email={email}'>Reset Password</a>";

            //Send Message
            var response = await _resend.EmailSendAsync(message);
            if (response.Exception != null )
            {
                return Result.Fail("Failed to send email, Error message = " + response.Exception.Message);
            }
            return Result.Ok();
        }

        //password reset execution
        public async Task<Result> Execute(string email, string newPassword)
        {
            var user = await _repo.GetByEmail(email);
            if (user == null)
            {
                return Result.Fail(new OwnerNotFoundError($"User not found for {email}"));
            }
            if (newPassword == null || newPassword.Length <= 6)
            {
                return Result.Fail("New password must be at least 6 characters long");
            }
            var newPasswordHash = _authService.HashPassword(newPassword);
            user.PasswordHash = newPasswordHash;
            await _repo.SaveChanges();
            return Result.Ok();
        }

    }
}
