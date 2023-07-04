using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private Recipe recipe;
        private List<Ingredient> originalIngredients; // Variable to store original ingredient quantities
        private RecipeManager recipeManager;
        private List<string> recipes = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            recipe = new Recipe();
            recipeManager = new RecipeManager();
        }

        private void UpdateRecipeList()
        {
            recipesListBox.Items.Clear();
            List<Recipe> sortedRecipes = recipeManager.GetRecipesSortedByName();
            foreach (Recipe recipe in sortedRecipes)
            {
                recipesListBox.Items.Add(recipe.Name);
            }
        }


        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(recipeNameTextBox.Text))
            {
                recipe.Name = recipeNameTextBox.Text;
                recipeManager.AddRecipe(recipe);
                UpdateRecipeList();
                ClearRecipeButton_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            ingredientDetailsPanel.Visibility = Visibility.Visible;
            IngredientDialog dialog = new IngredientDialog();
            if (dialog.ShowDialog() == true)
            {
                Ingredient ingredient = dialog.Ingredient;
                recipe.Ingredients.Add(ingredient);
                UpdateIngredientsList();
                UpdateCalories();
            }
        }
        private void SaveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ingredientNameTextBox.Text;
            string quantity = ingredientQuantityTextBox.Text;
            string unit = ingredientUnitTextBox.Text;
            string calories = ingredientCaloriesTextBox.Text;
            string foodGroup = ingredientFoodGroupTextBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(unit) ||
                string.IsNullOrEmpty(calories) || string.IsNullOrEmpty(foodGroup))
            {
                MessageBox.Show("Please enter all the ingredient information.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Save the ingredient details or perform any other necessary actions
                // Clear the text boxes and hide the ingredient details panel
                ingredientNameTextBox.Text = string.Empty;
                ingredientQuantityTextBox.Text = string.Empty;
                ingredientUnitTextBox.Text = string.Empty;
                ingredientCaloriesTextBox.Text = string.Empty;
                ingredientFoodGroupTextBox.Text = string.Empty;
                ingredientDetailsPanel.Visibility = Visibility.Collapsed;

                // Add the ingredient to the ingredientsListBox or perform any other necessary actions
            }
        }

        private void AddStepButton_Click(object sender, RoutedEventArgs e)
        {
            StepDialog dialog = new StepDialog();
            if (dialog.ShowDialog() == true)
            {
                string step = dialog.Step;
                recipe.Steps.Add(step);
                UpdateStepsList();
            }
        }
        private void RecipesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (recipesListBox.SelectedItem != null)
            {
                string selectedRecipeName = recipesListBox.SelectedItem.ToString();
                Recipe selectedRecipe = recipeManager.GetRecipesSortedByName().FirstOrDefault(r => r.Name == selectedRecipeName);
                if (selectedRecipe != null)
                {
                    recipeNameTextBox.Text = selectedRecipe.Name;
                    recipe.Ingredients = selectedRecipe.Ingredients;
                    recipe.Steps = selectedRecipe.Steps;
                    UpdateIngredientsList();
                    UpdateStepsList();
                    UpdateCalories();
                }
            }
        }

        private void ClearRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            // Store the current recipe
            string currentRecipe = GetRecipeAsString();

            // Clear the recipe
            recipe.ClearRecipe();
            recipeNameTextBox.Text = "";
            ingredientsListBox.Items.Clear();
            stepsListBox.Items.Clear();
            totalCaloriesTextBlock.Text = "";
            maxCaloriesIngredientTextBlock.Text = "";
            recipeTextBox.Text = "";

            // Add the current recipe to the list of recipes
            recipes.Add(currentRecipe);

            // Sort the recipes in alphabetical order
            recipes.Sort();

            // Clear the recipesListBox before displaying the recipes
            recipesListBox.Items.Clear();

            // Add each recipe to the recipesListBox
            foreach (string recipe in recipes)
            {
                recipesListBox.Items.Add(recipe);
            }
        }

        private string GetRecipeAsString()
        {
            // Convert the recipe information to a string representation
            string recipeName = recipeNameTextBox.Text;
            string ingredients = string.Join(", ", ingredientsListBox.Items.Cast<string>());
            string steps = string.Join("\n", stepsListBox.Items.Cast<string>());

            return $"Recipe Name: {recipeName}\nIngredients: \n{ingredients} \nSteps: \n{steps}";
        }

        private void ScaleRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            ScaleDialog dialog = new ScaleDialog();
            if (dialog.ShowDialog() == true)
            {
                double factor = dialog.ScaleFactor;

                // Check if the scale factor is one of the allowed values
                if (factor == 0.5 || factor == 2 || factor == 3)
                {
                    // Store the original ingredient quantities before scaling
                    originalIngredients = new List<Ingredient>();
                    foreach (Ingredient ingredient in recipe.Ingredients)
                    {
                        originalIngredients.Add(new Ingredient
                        {
                            Name = ingredient.Name,
                            Quantity = ingredient.Quantity,
                            UnitOfMeasurement = ingredient.UnitOfMeasurement,
                            Calories = ingredient.Calories,
                            FoodGroup = ingredient.FoodGroup
                        });
                    }

                    recipe.ScaleRecipe(factor);
                    UpdateIngredientsList();
                    UpdateCalories();
                }
                else
                {
                    MessageBox.Show("Invalid scale factor. Please enter 0.5, 2, or 3.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ResetQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                recipe.Ingredients[i].Quantity = originalIngredients[i].Quantity;
            }

            UpdateIngredientsList(); // Update the displayed ingredient list after resetting quantities
        }
        private void ExitApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user to confirm the exit
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the application?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // If the user confirms the exit, close the application
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void FilterByIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = filterIngredientTextBox.Text;
            List<Recipe> filteredRecipes = recipeManager.GetRecipesSortedByName()
                .Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredient)))
                .ToList();

            if (filteredRecipes.Count > 0)
            {
                recipesListBox.Items.Clear();
                foreach (Recipe recipe in filteredRecipes)
                {
                    recipesListBox.Items.Add(recipe.Name);
                }
            }
            else
            {
                MessageBox.Show("No recipes found for the specified ingredient.", "Recipe Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FilterByFoodGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string foodGroup = filterFoodGroupTextBox.Text;
            List<Recipe> filteredRecipes = recipeManager.GetRecipesSortedByName()
                .Where(r => r.Ingredients.Any(i => i.FoodGroup == foodGroup))
                .ToList();

            if (filteredRecipes.Count > 0)
            {
                recipesListBox.Items.Clear();
                foreach (Recipe recipe in filteredRecipes)
                {
                    recipesListBox.Items.Add(recipe.Name);
                }
            }
            else
            {
                MessageBox.Show("No recipes found for the specified food group.", "Recipe Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FilterByCaloriesButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(filterCaloriesTextBox.Text, out int maxCalories))
            {
                List<Recipe> filteredRecipes = recipeManager.GetRecipesSortedByName()
                    .Where(r => r.CalculateTotalCalories() <= maxCalories)
                    .ToList();

                if (filteredRecipes.Count > 0)
                {
                    recipesListBox.Items.Clear();
                    foreach (Recipe recipe in filteredRecipes)
                    {
                        recipesListBox.Items.Add(recipe.Name);
                    }
                }
                else
                {
                    MessageBox.Show("No recipes found within the specified calorie limit.", "Recipe Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }





        private void DisplayRecipeDetails(Recipe recipe)
        {
            // Display ingredients
            ingredientsListBox.Items.Clear();
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                ingredientsListBox.Items.Add(ingredient.Name);
            }

            // Display steps
            stepsListBox.Items.Clear();
            foreach (string step in recipe.Steps)
            {
                stepsListBox.Items.Add(step);
            }
        }

        private void UpdateIngredientsList()
        {
            ingredientsListBox.Items.Clear();
            foreach (Ingredient ingredient in recipe.Ingredients)
            {
                ingredientsListBox.Items.Add($"{ingredient.Quantity} {ingredient.UnitOfMeasurement} {ingredient.Name}");
            }
        }

        private void UpdateStepsList()
        {
            stepsListBox.Items.Clear();
            foreach (string step in recipe.Steps)
            {
                stepsListBox.Items.Add(step);
            }
        }

        private void UpdateCalories()
        {
            double totalCalories = recipe.CalculateTotalCalories();
            totalCaloriesTextBlock.Text = totalCalories.ToString();

            if (totalCalories > 300)
            {
                maxCaloriesIngredientTextBlock.Text = $"{recipe.GetIngredientWithMaxCalories()?.Name} ({recipe.GetIngredientWithMaxCalories()?.Calories} calories)";

                // Notify the user
                MessageBox.Show("Warning: The total calories of the recipe exceed 300.", "Calorie Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                maxCaloriesIngredientTextBlock.Text = "";
            }
        }

    }
    public class RecipeManager
    {
        private List<Recipe> recipes;

        public RecipeManager()
        {
            recipes = new List<Recipe>();
        }

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public List<Recipe> GetRecipesSortedByName()
        {
            return recipes.OrderBy(r => r.Name).ToList();
        }
    }
    // Custom Dialog Windows

    public class IngredientDialog : Window
    {
        public Ingredient Ingredient { get; private set; }

        public IngredientDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Title = "Add Ingredient";
            Width = 400;
            Height = 300;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row2);

            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row3);

            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row4);

            RowDefinition row5 = new RowDefinition();
            row5.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row5);

            RowDefinition row6 = new RowDefinition();
            row6.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row6);

            Label nameLabel = new Label();
            nameLabel.Content = "Ingredient Name:";
            nameLabel.Margin = new Thickness(5);
            Grid.SetRow(nameLabel, 0);
            grid.Children.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Margin = new Thickness(5);
            Grid.SetRow(nameTextBox, 0);
            Grid.SetColumn(nameTextBox, 1);
            grid.Children.Add(nameTextBox);

            Label quantityLabel = new Label();
            quantityLabel.Content = "Quantity:";
            quantityLabel.Margin = new Thickness(5);
            Grid.SetRow(quantityLabel, 1);
            grid.Children.Add(quantityLabel);

            TextBox quantityTextBox = new TextBox();
            quantityTextBox.Margin = new Thickness(5);
            Grid.SetRow(quantityTextBox, 1);
            Grid.SetColumn(quantityTextBox, 1);
            grid.Children.Add(quantityTextBox);

            Label unitLabel = new Label();
            unitLabel.Content = "Unit of Measurement:";
            unitLabel.Margin = new Thickness(5);
            Grid.SetRow(unitLabel, 2);
            grid.Children.Add(unitLabel);

            TextBox unitTextBox = new TextBox();
            unitTextBox.Margin = new Thickness(5);
            Grid.SetRow(unitTextBox, 2);
            Grid.SetColumn(unitTextBox, 1);
            grid.Children.Add(unitTextBox);

            Label caloriesLabel = new Label();
            caloriesLabel.Content = "Calories:";
            caloriesLabel.Margin = new Thickness(5);
            Grid.SetRow(caloriesLabel, 3);
            grid.Children.Add(caloriesLabel);

            TextBox caloriesTextBox = new TextBox();
            caloriesTextBox.Margin = new Thickness(5);
            Grid.SetRow(caloriesTextBox, 3);
            Grid.SetColumn(caloriesTextBox, 1);
            grid.Children.Add(caloriesTextBox);

            Label foodGroupLabel = new Label();
            foodGroupLabel.Content = "Food Group:";
            foodGroupLabel.Margin = new Thickness(5);
            Grid.SetRow(foodGroupLabel, 4);
            grid.Children.Add(foodGroupLabel);

            TextBox foodGroupTextBox = new TextBox();
            foodGroupTextBox.Margin = new Thickness(5);
            Grid.SetRow(foodGroupTextBox, 4);
            Grid.SetColumn(foodGroupTextBox, 1);
            grid.Children.Add(foodGroupTextBox);

            Button addButton = new Button();
            addButton.Content = "Add";
            addButton.Margin = new Thickness(5);
            addButton.HorizontalAlignment = HorizontalAlignment.Right;
            addButton.Click += (sender, e) =>
            {
                if (double.TryParse(quantityTextBox.Text, out double quantity) && double.TryParse(caloriesTextBox.Text, out double calories))
                {
                    Ingredient = new Ingredient
                    {
                        Name = nameTextBox.Text,
                        Quantity = quantity,
                        UnitOfMeasurement = unitTextBox.Text,
                        Calories = calories,
                        FoodGroup = foodGroupTextBox.Text
                    };
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid quantity or calories. Please enter valid numbers.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            Grid.SetRow(addButton, 5);
            Grid.SetColumnSpan(addButton, 2);
            grid.Children.Add(addButton);

            Content = grid;
        }
    }


    public class StepDialog : Window
    {
        public string Step { get; private set; }

        public StepDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Title = "Add Step";
            Width = 300;
            Height = 200;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row2);

            TextBox stepTextBox = new TextBox();
            stepTextBox.AcceptsReturn = true;
            stepTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            stepTextBox.Margin = new Thickness(5);
            Grid.SetRow(stepTextBox, 0);
            grid.Children.Add(stepTextBox);

            Button addButton = new Button();
            addButton.Content = "Add";
            addButton.Margin = new Thickness(5);
            addButton.HorizontalAlignment = HorizontalAlignment.Right;
            addButton.Click += (sender, e) =>
            {
                Step = stepTextBox.Text;
                DialogResult = true;
            };
            Grid.SetRow(addButton, 1);
            grid.Children.Add(addButton);

            Content = grid;
        }
    }


    public class ScaleDialog : Window
    {
        public double ScaleFactor { get; private set; }

        public ScaleDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Title = "Scale Recipe";
            Width = 300;
            Height = 200;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Auto);
            grid.RowDefinitions.Add(row2);

            TextBox scaleTextBox = new TextBox();
            scaleTextBox.Margin = new Thickness(5);
            Grid.SetRow(scaleTextBox, 0);
            grid.Children.Add(scaleTextBox);

            Button scaleButton = new Button();
            scaleButton.Content = "Scale";
            scaleButton.Margin = new Thickness(5);
            scaleButton.HorizontalAlignment = HorizontalAlignment.Right;
            scaleButton.Click += (sender, e) =>
            {
                if (double.TryParse(scaleTextBox.Text, out double scaleFactor))
                {
                    ScaleFactor = scaleFactor;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Invalid scale factor. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            Grid.SetRow(scaleButton, 1);
            grid.Children.Add(scaleButton);

            Content = grid;
        }
    }
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }

        public double CalculateTotalCalories()
        {
            double totalCalories = 0;
            foreach (Ingredient ingredient in Ingredients)
            {
                totalCalories += ingredient.Calories;
            }
            return totalCalories;
        }

        public Ingredient GetIngredientWithMaxCalories()
        {
            Ingredient maxCaloriesIngredient = null;
            double maxCalories = 0;
            foreach (Ingredient ingredient in Ingredients)
            {
                if (ingredient.Calories > maxCalories)
                {
                    maxCalories = ingredient.Calories;
                    maxCaloriesIngredient = ingredient;
                }
            }
            return maxCaloriesIngredient;
        }

        public void ClearRecipe()
        {
            Name = "";
            Ingredients.Clear();
            Steps.Clear();
        }

        public void ScaleRecipe(double factor)
        {
            foreach (Ingredient ingredient in Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

    }
}
