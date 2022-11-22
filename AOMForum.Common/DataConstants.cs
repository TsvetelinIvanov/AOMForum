namespace AOMForum.Common
{
    public static class DataConstants
    {
        public const int IdStringMaxLength = 40;
        public const int ShortContentMaxLength = 60;

        public static class ApplicationUser
        {
            public const int UserNameMinLength = 3;
            public const int UserNameMaxLength = 30;
            public const int EmailMinLength = 8;
            public const int EmailMaxLength = 60;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
            public const int PhoneNumberMinLength = 4;
            public const int PhoneNumberMaxLength = 40;

            public const int FirstNameMinLength = 1;
            public const int FirstNameMaxLength = 30;
            public const int SecondNameMinLength = 1;
            public const int SecondNameMaxLength = 30;
            public const int LastNameMinLength = 1;
            public const int LastNameMaxLength = 30;

            public const int BiographyMinLength = 30;
            public const int BiographyMaxLength = 600;

            public const int AgeMinValue = 18;
            public const int AgeMaxValue = 130;
        }

        public static class Category
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 60;

            public const int DescriptionMinLength = 6;
            public const int DescriptionMaxLength = 1000;
        }

        public static class Tag
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }        

        public static class Post
        {
            public const int TitleMinLength = 1;
            public const int TitleMaxLength = 60;
            
            public const int ContentMinLength = 10;
            public const int ContentMaxLength = 10000;
        }

        public static class PostReport
        {
            public const int ContentMinLength = 3;
            public const int ContentMaxLength = 1000;
        }

        public static class Comment
        {
            public const int ContentMinLength = 3;
            public const int ContentMaxLength = 10000;
        }

        public static class CommentReport
        {
            public const int ContentMinLength = 3;
            public const int ContentMaxLength = 1000;
        }        

        public static class Message
        {
            public const int ContentMinLength = 1;
            public const int ContentMaxLength = 1000;
        }

        public static class Setting
        {
            public const int NameMinLength = 1;
            public const int NameMaxLength = 60;

            public const int ContentMinLength = 1;
            public const int ContentMaxLength = 10000;
        }
    }
}