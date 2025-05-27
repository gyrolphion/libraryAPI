using libraryapi.Objects.ReturnObject;
using libraryapi.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace libraryapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {

        [HttpPost("login")]
        public LoginData Login(string username, string password)
        {

            var data = Database.GetDataTable(
                "select * from users where username = @username and password = @password",
                new Microsoft.Data.SqlClient.SqlParameter("@username", username),
                new Microsoft.Data.SqlClient.SqlParameter("@password", password)
                );

            if (data.Rows.Count > 0)
            {

                User user = Database.toData<User>(data.Rows[0]);

                return new LoginData(true, TokenCreator.CreateToken(user.userID), "Login is success");

            }

            return new LoginData(false, "", "Username or password is wrong");

        }

        [HttpPost("register")]
        public IActionResult Register(string username, string password, string mail)
        {
            var existingUser = Database.GetDataTable(
                "SELECT * FROM users WHERE username = @username OR mail = @mail",
                new SqlParameter("@username", username),
                new SqlParameter("@mail", mail)
            );

            if (existingUser.Rows.Count > 0)
            {
                return BadRequest(new { status = false, message = "Username or email already exists" });
            }

            Database.ExecuteCommand(
                "INSERT INTO users (username, password, mail) VALUES (@username, @password, @mail)",
                new SqlParameter("@username", username),
                new SqlParameter("@password", password),
                new SqlParameter("@mail", mail)
            );

            return Ok(new { status = true, message = "Registration successful" });
        }



        [HttpGet("getbooks")]
        public Book[] GetBooks()
        {
            var data = Database.GetDataTable("select * from books");

            Book[] books = Database.ToList<Book>(data).ToArray();

            return books;
        }

        [HttpGet("getbook")]
        public Book[] searchingbook(string name)
        {
            var data = Database.GetDataTable(
                "SELECT * FROM books WHERE bookName LIKE @bookName",
                new Microsoft.Data.SqlClient.SqlParameter("bookName", "%" + name + "%")
            );

            Book[] books = Database.ToList<Book>(data).ToArray();

            return books;

        }

        [HttpGet("getauthor")]
        public Book[] searchingauthor(string name)
        {
            var data = Database.GetDataTable(
                "SELECT * FROM books WHERE authorName LIKE @authorName",
                new Microsoft.Data.SqlClient.SqlParameter("authorName", "%" + name + "%")
            );


            Book[] books = Database.ToList<Book>(data).ToArray();

            return books;

        }

        [HttpGet("setComment")]
        public ReturnMessage SetComment(string Token, int bookID, string title, string comment, int point)
        {
            var userData = Database.GetDataTable(
                "SELECT userID FROM tokens WHERE tokens.token = @token",
                new SqlParameter("@token", Token)
            );

            if (userData.Rows.Count > 0)
            {
                var userID = userData.Rows[0]["userID"];

                var kontrol = Database.GetDataTable(
                    "SELECT * FROM comments WHERE bookID = @bookID AND userID = @userID",
                    new SqlParameter("@bookID", bookID),
                    new SqlParameter("@userID", userID)
                );

                if (kontrol.Rows.Count == 0)
                {
                    Database.ExecuteCommand(
                        "INSERT INTO comments (bookID, userID, title, text) VALUES (@bookID, @userID, @title, @text )",
                        new SqlParameter("@bookID", bookID),
                        new SqlParameter("@userID", userID),
                        new SqlParameter("@title", title),
                        new SqlParameter("@text", comment),
                        new SqlParameter("@point", point)
                    );

                    Database.ExecuteCommand(
                        "INSERT INTO points (userID, bookID, point) VALUES (@userID, @bookID, @point)",
                        new SqlParameter("@userID", userID),
                        new SqlParameter("@bookID", bookID),
                        new SqlParameter("@point", point)
                    );

                    return new ReturnMessage(true, "Comment is success");
                }
                else
                {
                    return new ReturnMessage(false, "You already commented");
                }
            }

            return new ReturnMessage(false, "Invalid token");
        }

        [HttpGet("setPoint")]
        public ReturnMessage SetPoint(string Token, int bookID, int point)
        {
            var userData = Database.GetDataTable(
                "SELECT users.userID FROM tokens INNER JOIN users ON tokens.userID = users.userID WHERE tokens.token = @token",
                new SqlParameter("@token", Token)
            );

            try
            {

                if (userData.Rows.Count > 0)
                {
                    var userID = userData.Rows[0]["userID"];

                    Database.ExecuteCommand(
                        "INSERT INTO points (userID, bookID, point) VALUES (@userID, @bookID, @point)",
                        new SqlParameter("@userID", userID),
                        new SqlParameter("@bookID", bookID),
                        new SqlParameter("@point", point)
                    );
                }

                return new ReturnMessage(true, "");

            }
            catch (Exception)
            {

                return new ReturnMessage(false, "This book has already been commented or rated..");

            }

        }

        [HttpPost("updateComment")]
        public ReturnMessage UpdateComment(string Token, int bookID, string newTitle, string newComment, int newPoint)
        {
            var userData = Database.GetDataTable(
                "SELECT userID FROM tokens WHERE tokens.token = @token",
                new SqlParameter("@token", Token)
            );

            if (userData.Rows.Count > 0)
            {
                var userID = userData.Rows[0]["userID"];

                var existingComment = Database.GetDataTable(
                    "SELECT * FROM comments WHERE userID = @userID AND bookID = @bookID",
                    new SqlParameter("@userID", userID),
                    new SqlParameter("@bookID", bookID)
                );

                if (existingComment.Rows.Count > 0)
                {
                    Database.ExecuteCommand(
                        "UPDATE comments SET title = @newTitle, text = @newText WHERE userID = @userID AND bookID = @bookID",
                        new SqlParameter("@newTitle", newTitle),
                        new SqlParameter("@newText", newComment), 
                        new SqlParameter("@userID", userID),
                        new SqlParameter("@bookID", bookID)
                    );

                    Database.ExecuteCommand(
                        "UPDATE points SET point = @newPoint WHERE userID = @userID AND bookID = @bookID",
                        new SqlParameter("@newPoint", newPoint),
                        new SqlParameter("@userID", userID),
                        new SqlParameter("@bookID", bookID)
                    );

                    return new ReturnMessage(true, "Comment updated successfully");
                }
                else
                {
                    return new ReturnMessage(false, "No existing comment found to update");
                }
            }

            return new ReturnMessage(false, "Invalid token");
        }



    }
}
