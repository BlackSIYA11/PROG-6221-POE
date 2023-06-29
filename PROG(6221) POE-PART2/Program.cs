using System;
using System.Collections.Generic;

class Recipe
{
    // Using a delegate to store the recipe class
    public delegate void CaloriesExceededHandler(Recipe recipe);
    public event CaloriesExceededHandler CaloriesExceeded;

    private string name;
    private List<Ingredient> originalIngredients;
    private List<Ingredient> ingredients;
    private List<string> steps;

    public Recipe()
    {
        originalIngredients = new List<Ingredient>();
        ingredients = new List<Ingredient>();
        steps = new List<string>();
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public List<Ingredient> Ingredients
    {
        get { return ingredients; }
    }

    public List<string> Steps
    {
        get { return steps; }
    }

    public void EnterIngredients()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Enter ingredients (enter 'done' to finish)");
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Enter ingredient details:");

            Ingredient ingredient = new Ingredient();

            Console.Write("Name: ");
            ingredient.Name = Console.ReadLine();

            if (ingredient.Name.ToLower() == "done")
                break;

            Console.Write("Quantity: ");
            ingredient.Quantity = Convert.ToDouble(Console.ReadLine());

            Console.Write("Unit of measurement: ");
            ingredient.UnitOfMeasurement = Console.ReadLine();

            Console.Write("Calories: ");
            ingredient.Calories = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the food group:");
            ingredient.FoodGroup = Console.ReadLine();

            ingredients.Add(ingredient);
            originalIngredients.Add(new Ingredient
            {
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                UnitOfMeasurement = ingredient.UnitOfMeasurement,
                Calories = ingredient.Calories,
                FoodGroup = ingredient.FoodGroup
            });
        }
    }

    public void EnterSteps()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Enter steps (enter 'done' to finish)");
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Enter step: ");
            string step = Console.ReadLine();

            if (step.ToLower() == "done")
                break;

            steps.Add(step);
        }
    }

    // Event handler method for calories exceeded event
    protected virtual void OnCaloriesExceeded()
    {
        if (CaloriesExceeded != null)
        {
            CaloriesExceeded(this); // Raise the event
        }
    }

    public void DisplayRecipe()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Recipe: " + name);
        Console.WriteLine("--------");
        Console.WriteLine("Ingredients:");
        Console.WriteLine("-----------");
        foreach (Ingredient ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.Quantity} {ingredient.UnitOfMeasurement} {ingredient.Name}");
        }
        Console.WriteLine();

        Console.WriteLine("Steps:");
        Console.WriteLine("--------");
        for (int i = 0; i < steps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {steps[i]}");
        }
        Console.WriteLine();

        double totalCalories = CalculateTotalCalories();
        Console.WriteLine("Total Calories: " + totalCalories);
        Console.WriteLine("The amount of Calories should be less than 300 for each recipe");

        if (totalCalories > 300)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Warning: Total calories exceed 300!");
            Ingredient ingredientWithMaxCalories = GetIngredientWithMaxCalories();
            Console.WriteLine($"Ingredient with highest calories: {ingredientWithMaxCalories.Name} ({ingredientWithMaxCalories.Calories} calories)");
        }
        Console.WriteLine();
    }

    // Calculate the total amount of calories in every ingredient for a recipe
    public double CalculateTotalCalories()
    {
        double totalCalories = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
    }

    // Get the ingredient with the highest number of calories
    public Ingredient GetIngredientWithMaxCalories()
    {
        Ingredient ingredientWithMaxCalories = null;
        double maxCalories = 0;

        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.Calories > maxCalories)
            {
                maxCalories = ingredient.Calories;
                ingredientWithMaxCalories = ingredient;
            }
        }

        return ingredientWithMaxCalories;
    }

    public void ScaleRecipe(double factor)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            ingredient.Quantity *= factor;
        }
    }

    public void ResetQuantities()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            ingredients[i].Quantity = originalIngredients[i].Quantity;
        }
    }

    public void ClearRecipe()
    {
        ingredients.Clear();
        steps.Clear();
        originalIngredients.Clear(); // Clear the list of original ingredients

        Console.WriteLine("Recipe has been cleared.");

        Console.WriteLine("Enter recipe details");

        Console.Write("Name of Recipe: ");
        name = Console.ReadLine();

        EnterIngredients();
        EnterSteps();
    }
}

class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string UnitOfMeasurement { get; set; }
    public double Calories { get; set; }
    public string FoodGroup { get; set; }
}

class Program
{
    public static void Main(string[] args)
    {


        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        List<Recipe> recipes = new List<Recipe>();
        Console.WriteLine("WELCOME TO MY APPLICATION");
        Console.WriteLine("-------------------------");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Enter recipe details");
            Recipe recipe = new Recipe();

            Console.Write("Name of Recipe: ");
            recipe.Name = Console.ReadLine();

            // Subscribe to the CaloriesExceeded event
            recipe.CaloriesExceeded += Recipe_CaloriesExceeded;

            recipe.EnterIngredients();
            recipe.EnterSteps();

            recipes.Add(recipe);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Recipe has been added!");

            Console.WriteLine("Do you want to add another recipe? (y/n)");
            string input = Console.ReadLine();

            if (input.ToLower() != "y")
                break;
        }

        // Using the sort function to sort the recipe names and display them in alphabetical order
        recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("All Recipes:");
        Console.WriteLine("------------");
        for (int i = 0; i < recipes.Count; i++)
        {
            Console.WriteLine($"Recipe #{i + 1}: {recipes[i].Name}");
        }

        Console.WriteLine("Enter the recipe number to display: ");
        int recipeNumber = Convert.ToInt32(Console.ReadLine());

        if (recipeNumber > 0 && recipeNumber <= recipes.Count)
        {
            Recipe selectedRecipe = recipes[recipeNumber - 1];
            Console.WriteLine();
            selectedRecipe.DisplayRecipe();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Enter 's' to scale the recipe, 'r' to reset the quantities, 'c' to clear the recipe and start over, or 'q' to quit the application: ");
                string input = Console.ReadLine();

                if (input == "s")
                {
                    Console.Write("Enter scaling factor (0.5, 2, or 3): ");
                    double factor = Convert.ToDouble(Console.ReadLine());
                    selectedRecipe.ScaleRecipe(factor);
                    selectedRecipe.DisplayRecipe();
                }
                else if (input == "r")
                {
                    selectedRecipe.ResetQuantities();
                    selectedRecipe.DisplayRecipe();
                }
                else if (input == "c")
                {
                    selectedRecipe.ClearRecipe();
                    recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
                    Console.WriteLine("All Recipes:");
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        Console.WriteLine($"Recipe #{i + 1}: {recipes[i].Name}");
                    }
                    Console.WriteLine("Enter the recipe number to display: ");
                    recipeNumber = Convert.ToInt32(Console.ReadLine());
                    if (recipeNumber > 0 && recipeNumber <= recipes.Count)
                    {
                        selectedRecipe = recipes[recipeNumber - 1];
                        Console.WriteLine();
                        selectedRecipe.DisplayRecipe();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Option!!!");
                        break;
                    }
                }
                if (input == "q")
                {
                    Console.WriteLine("GOODBYE!!");
                    break;
                }
            }

        }
        else
        {
            Console.WriteLine("Invalid Option!!!!");
        }
    }

    // Event handler for the CaloriesExceeded event
    private static void Recipe_CaloriesExceeded(Recipe recipe)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Warning: Calories in recipe '{recipe.Name}' exceed 300!");
    }
}
