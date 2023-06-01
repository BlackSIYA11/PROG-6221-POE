using System;
using System.Collections.Generic;

class Recipe
{
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
        Console.WriteLine("Enter ingredients (enter 'done' to finish)");
        while (true)
        {
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
        }
    }

    public void EnterSteps()
    {
        Console.WriteLine("Enter steps (enter 'done' to finish)");
        while (true)
        {
            Console.Write("Enter step: ");
            string step = Console.ReadLine();

            if (step.ToLower() == "done")
                break;

            
            steps.Add(step);
        }
    }

    public void DisplayRecipe()
    {
        Console.WriteLine("Recipe: " + name);
        Console.WriteLine("--------");

        foreach (Ingredient ingredient in ingredients)
        {
            Console.WriteLine($"{ingredient.Quantity} {ingredient.UnitOfMeasurement} {ingredient.Name}");
        }
        Console.WriteLine();

        Console.WriteLine("Steps:");
        for (int i = 0; i < steps.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {steps[i]}");
        }
        Console.WriteLine();

        double totalCalories = CalculateTotalCalories();
        Console.WriteLine("Total Calories: " + totalCalories);

        if (totalCalories > 300)
        {
            Console.WriteLine("Warning: Total calories exceed 300!");
        }
        Console.WriteLine();
    }

    public double CalculateTotalCalories()
    {
        double totalCalories = 0;
        foreach (Ingredient ingredient in ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
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

        foreach (Ingredient ingredient in originalIngredients)
        {
            ingredients.Add(new Ingredient
            {
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                UnitOfMeasurement = ingredient.UnitOfMeasurement,
                Calories = ingredient.Calories,
                FoodGroup = ingredient.FoodGroup
            });
        }

        foreach (string step in Steps)
        {
            steps.Add(step);
        }

        Console.WriteLine("Recipe has been cleared.");
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
    static void Main(string[] args)
    {
        List<Recipe> recipes = new List<Recipe>();

        while (true)
        {
            Console.WriteLine("Enter recipe details");
            Recipe recipe = new Recipe();

            Console.Write("Name of Recipe: ");
            recipe.Name = Console.ReadLine();

            recipe.EnterIngredients();
            recipe.EnterSteps();

            recipes.Add(recipe);

            Console.WriteLine("Recipe has been added!");

            Console.WriteLine("Do you want to add another recipe? (y/n)");
            string input = Console.ReadLine();

            if (input.ToLower() != "y")
                break;
        }

        recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));

        Console.WriteLine("All Recipes:");
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
                    selectedRecipe.EnterIngredients();
                    selectedRecipe.EnterSteps();
                    selectedRecipe.DisplayRecipe();
                }
                else if (input == "q")
                {
                    Console.WriteLine("GOODBYE!!");
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid recipe number!");
        }
    }
}
