using System;

class Recipe
{
    // Declaring the variables
    private string[] ingredients;
    private double[] quantities;
    private string[] units;
    private string[] steps;

    private double[] originalQuantities;

    public Recipe(int numIngredients, int numSteps)
    {
        // initialize the arrays
        ingredients = new string[numIngredients];
        quantities = new double[numIngredients];
        units = new string[numIngredients];
        steps = new string[numSteps];

        // initialize the originalQuantities array
        originalQuantities = new double[numIngredients];
    }

    public void EnterIngredients()
    {
        Console.WriteLine("WELCOME TO MY APPLICATION");
        Console.WriteLine("*************************");
        // Enter ingridients details
        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter details for ingredient #{i + 1}:");
            Console.Write("Name of Ingredient: ");
            ingredients[i] = Console.ReadLine();
            Console.Write("Quantity of ingredient: ");
            quantities[i] = Convert.ToDouble(Console.ReadLine());
            originalQuantities[i] = quantities[i]; // store original quantities
            Console.Write("Unit of measurement: ");
            units[i] = Console.ReadLine();
        }
    }

    public void EnterSteps()
    {
        
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
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Recipe:");
        Console.WriteLine("--------");

        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.WriteLine($"{quantities[i]} {units[i]} {ingredients[i]}");
        }
        Console.WriteLine();

        Console.WriteLine("Steps");
        Console.WriteLine("--------");
        for (int i = 0; i < steps.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {steps[i]}");
        }

        Console.WriteLine();
    }

    public void ScaleRecipe(double factor)
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            quantities[i] = originalQuantities[i] * factor;
        }
    }

    public void ResetQuantities()
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            quantities[i] = originalQuantities[i];
        }
    }

    public void ClearRecipe()
    {
        ingredients = new string[ingredients.Length];
        quantities = new double[quantities.Length];
        units = new string[units.Length];
        steps = new string[steps.Length];
        originalQuantities = new double[originalQuantities.Length];

        Console.WriteLine("Recipe has been cleared.");
    }
}


class Program
{
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("WELCOME TO MY APPLICATION");
        Console.WriteLine("-------------------------");
        Console.ForegroundColor = ConsoleColor.Yellow;
        // prompt the user to enter recipe details
        Console.Write("Enter the number of ingredients: ");
        int numIngredients = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter the number of steps: ");
        int numSteps = Convert.ToInt32(Console.ReadLine());

        Recipe recipe = new Recipe(numIngredients, numSteps);
        recipe.EnterIngredients();
        recipe.EnterSteps();
        recipe.DisplayRecipe();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Enter 's' to scale the recipe, 'r' to reset the quantities, 'c' to clear the recipe and start over, or if 'q' quit the application: ");
            string input = Console.ReadLine();

            if (input == "s")
            {
                Console.Write("Enter scaling factor (0.5, 2, or 3): ");
                double factor = double.Parse(Console.ReadLine());
                recipe.ScaleRecipe(factor);
                recipe.DisplayRecipe();
            }
            else if (input == "r")
            {
                recipe.ResetQuantities();
                recipe.DisplayRecipe();
            }
            else if (input == "c")
            {
                recipe.ClearRecipe();

                Console.Write("Enter the number of ingredients for the new recipe: ");
                numIngredients = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter the number of steps for the new recipe: ");
                numSteps = Convert.ToInt32(Console.ReadLine());

                recipe = new Recipe(numIngredients, numSteps);
                recipe.EnterIngredients();
                recipe.EnterSteps();
                recipe.DisplayRecipe();

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Enter 's' to scale the recipe, 'r' to reset the quantities, or 'q' to quit the application: ");
                    input = Console.ReadLine();

                    if (input == "s")
                    {
                        Console.Write("Enter scaling factor (0.5, 2, or 3): ");
                        double factor = Convert.ToDouble(Console.ReadLine());
                        recipe.ScaleRecipe(factor);
                        recipe.DisplayRecipe();
                    }
                    else if (input == "r")
                    {
                        recipe.ResetQuantities();
                        recipe.DisplayRecipe();
                    }
                    else if (input == "q")
                    {
                        break;
                    }
                }
            }
            else if (input == "q")
            {
                break;
            }
        }
    }
}
