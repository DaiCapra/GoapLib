namespace GoapLib.Tests.Planning
{
    public static class ActionFactory
    {
        public static Action<Attributes, bool> BuyBeans()
        {
            return new Action<Attributes, bool>()
                .AddName("Buy beans")
                .AddCondition(Attributes.HasMoney, true)
                .AddEffect(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasMoney, false);
        }

        public static Action<Attributes, bool> MakeCoffee()
        {
            return new Action<Attributes, bool>()
                .AddName("Make coffee")
                .AddCondition(Attributes.HasBeans, true)
                .AddEffect(Attributes.HasCoffee, true)
                .AddEffect(Attributes.HasBeans, false);
        }

        public static Action<Attributes, bool> BuyCoffee()
        {
            return new Action<Attributes, bool>()
                .AddName("Buy coffee")
                .AddCondition(Attributes.HasMoney, true)
                .AddEffect(Attributes.HasCoffee, true)
                .AddEffect(Attributes.HasMoney, false)
                .AddCost(10);
        }

        public static Action<Attributes, bool> DrinkCoffee()
        {
            return new Action<Attributes, bool>()
                .AddName("Drink coffee")
                .AddCondition(Attributes.HasCoffee, true)
                .AddEffect(Attributes.IsThirsty, false)
                .AddEffect(Attributes.HasCoffee, false);
        }
    }
}