namespace libraryapi
{
    public static class TokenCreator
    {

        private static char[] letters = Enumerable.Range('a', 26).Select(c => (char)c)
            .Concat(Enumerable.Range('A', 26).Select(c => (char)c))
            .Concat(Enumerable.Range('0', 10).Select(c => (char)c))
            .ToArray();


        public static string CreateToken()
        {
            Random random = new Random();
            string token = new string(Enumerable.Range(0, 64).Select(_ => letters[random.Next(letters.Length)]).ToArray());
            return token;
        }

        public static string CreateToken(int userID)
        {

            string token = CreateToken();

            Database.ExecuteCommand("insert into tokens(token, userID) values (@token, @userID)",
                new Microsoft.Data.SqlClient.SqlParameter("@token", token),
                new Microsoft.Data.SqlClient.SqlParameter("@userID", userID)
            );

            return token;
        
        }

    }
}
