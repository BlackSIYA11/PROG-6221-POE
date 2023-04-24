using System;

class Recipe
{
    private string[] ingredients;
    private double[] quantities;
    private string[] units;
    private string[] steps;

    public Recipe(int numIngredients, int numSteps)
    {
        // initialize the arrays
        ingredients = new string[numIngredients];
        quantities = new double[numIngredients];
        units = new string[numIngredients];
        steps = new string[numSteps];
    }

    public void EnterIngredients()
    {
        // Enter ingridients details
        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.WriteLine($"Enter details for ingredient {i + 1}:");
            Console.Write("Name of Ingridient: ");
            ingredients[i] = Console.ReadLine();
            Console.Write("Quantity of ingridient: ");
            quantities[i] = Convert.ToDouble(Console.ReadLine());
            Console.Write("Unit of measurement: ");
            units[i] = Console.ReadLine();
        }
    }

    public void EnterSteps()
    {
        Console.Write("Enter the number of steps: ");
        int numSteps = Convert.ToInt32(Console.ReadLine());

        // get recipe steps
        for (int i = 0; i < steps.Length; i++)
        {
            Console.Write($"Enter step {i + 1}: ");
            steps[i] = Console.ReadLine();
        }
    }
  public void DisplayRecipe()
    {
        Console.WriteLine("Recipe:");
        Console.WriteLine("--------");

        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.WriteLine($"{quantities[i]} {units[i]} {ingredients[i]}");
        }
        Console.WriteLine();

        for (int i = 0; i < steps.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {steps[i]}");
        }

        Console.WriteLine();
    }


    public void ScaleRecipe(double factor)
    {
        for (int i = 1; i < quantities.Length; i++)
        {
            quantities[i] *= factor;
        }
    }
    public void ResetQuantities()
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            quantities[i] = originalQuantities[i];
        }
    }

    public void ClearData()
    {
        ingredients = new string[ingredients.Length];
        quantities = new double[quantities.Length];
        units = new string[units.Length];
        steps = new string[steps.Length];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the number of ingredients:");
        int numIngredients = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the number of steps:");
        int numSteps = int.Parse(Console.ReadLine());

        Recipe myrecipe = new Recipe(numIngredients, numSteps);

        myrecipe.Ingredients();
        myrecipe.Steps();
        myrecipe.DisplayRecipe();

        Console.WriteLine("\nEnter scaling factor (0.5, 2, or 3):");
        double factor = Convert.ToDouble(Console.ReadLine());
        myrecipe.ScaleRecipe(factor);
        myrecipe.DisplayRecipe();


        myrecipe.ResetQuantities();
        myrecipe.DisplayRecipe();

        myrecipe.ClearData();
    }
}
