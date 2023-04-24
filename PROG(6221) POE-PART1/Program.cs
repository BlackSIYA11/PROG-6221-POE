﻿using System;

class Recipe
{
    // Declaring variables arrays
    private string[] ingredients;
    private double[] quantities;
    private double[] originalQuantities;
    private string[] units;
    private string[] steps;

    public Recipe(int numIngredients, int numSteps)
    {
        ingredients = new string[numIngredients];
        quantities = new double[numIngredients];
        originalQuantities = new double[numIngredients];
        units = new string[numIngredients];
        steps = new string[numSteps];
    }

    public void Ingredients()
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.WriteLine("Enter ingredient #{0}", i + 1);
            ingredients[i] = Console.ReadLine();
            Console.WriteLine("Enter quantity for {0}", ingredients[i]);
            quantities[i] = Convert.ToDouble(Console.ReadLine());
            originalQuantities[i] = quantities[i];
            Console.WriteLine("Enter unit of measurement for {0}", ingredients[i]);
            units[i] = Console.ReadLine();
        }
    }

    public void Steps()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            Console.WriteLine("Enter step #{0}", i + 1);
            steps[i] = Console.ReadLine();
        }
    }

    public void DisplayRecipe()
    {
        Console.WriteLine("Ingredients:");
        for (int i = 0; i < ingredients.Length; i++)
        {
            Console.WriteLine("{0} {1} {2}", quantities[i], units[i], ingredients[i]);
        }

        Console.WriteLine("\nSteps:");
        for (int i = 0; i < steps.Length; i++)
        {
            Console.WriteLine("{0}. {1}", i + 1, steps[i]);
        }
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
