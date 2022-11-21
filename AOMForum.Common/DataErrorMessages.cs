namespace AOMForum.Common
{
    public static class DataErrorMessages
    {
        public const string RequiredErrorMessage = "Моля въведете \"{0}\"!";

        public const string StringLengthErrorMessage = "\"{0}\" трябва да бъде с дължина между \"{2}\" и \"{1}\" знака!";

        public const string ConfirmPasswordErrorMessage = "Паролата и потвърждението и се различават!";
        public const string EmailErrorMessage = "Адресът на електронната поща трябва да бъде във валиден формат!";
        public const string PhoneNumberErrorMessage = "Телефонният номер трябва да бъде във валиден формат!";
        public const string AgeErrorMessage = "Възръстта трябва да бъде между {0} и {1} години!";
        public const string AgeRestrictionErrorMessage = "Трябва да имате навършени поне {0} години!";
        public const string InvalidGenderType = "Невалиден пол. Моля изберете един от посочените!";
        public const string ProfilePictureUploadErrorMessage = "Невалиден формат на файла! Разрешените файлви формати са: \"jpg\", \"jpeg\", \"png\" и \"gif\".";        

        public const string CategoryExistingNameErrorMessage = "В този форум вече същесвува категория с такова име. Моля изберете друго!";
        public const string CategoryNonExistingIdErrorMessage = "Такава категория не съществува в този форум!";
        
        public const string TagIsRequiredErrorMessage = "Трябва да бъде избран поне един таг!";
        public const string TagExistingNameErrorMessage = "В този форум вече същесвува таг с такова име. Моля изберете друго!";
        public const string TagNonExistingIdErrorMessage = "Такъв таг не съществува в този форум!";
    }
}