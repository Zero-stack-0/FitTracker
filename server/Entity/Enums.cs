namespace Entity
{
    public class Enums
    {
        public enum UserRole
        {
            Admin = 1,
            User = 2
        }

        public enum Gender
        {
            Male = 1,
            Female = 2,
            Other = 3
        }

        public enum FintnessGoal
        {
            WeightLoss = 1,
            MuscleGain,
            MaintainWeight,
            WeightGain
        }

        public enum ActivityLevel
        {
            Sedentary = 1,
            LightlyActive,
            ModeratelyActive,
            VeryActive,
            SuperActive
        }

        public enum AiPromptType
        {
            Advance = 1,
            Basic = 2
        }

        public enum DIET_TYPE
        {
            VEG = 1,
            NON_VEG = 2,
            BOTH
        }

        public enum TimeOfTheDay
        {
            Breakfast = 1,
            Lunch = 2,
            Dinner = 3,
            Snack = 4
        }

        public enum EMAIL_TEMPLATE_TYPE
        {
            Verification = 1,
            Forgot_Password = 2
        }
    }
}