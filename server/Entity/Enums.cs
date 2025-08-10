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
    }
}