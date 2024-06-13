using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes; // List to store all recipes
        private Recipe currentRecipe; // The currently selected or newly created recipe
        private List<Ingredient> originalIngredients; // To store original ingredient quantities for resetting

        public MainWindow()
        {
            InitializeComponent();
            recipes = new List<Recipe>();
            currentRecipe = new Recipe();
            originalIngredients = new List<Ingredient>();
            UpdateRecipeList();
        }

        // Adds ingredient input fields based on the entered count
        private void AddIngredients_Click(object sender, RoutedEventArgs e)
        {
            int count;
            if (int.TryParse(IngredientsCountTextBox.Text, out count) && count > 0)
            {
                IngredientsPanel.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    StackPanel ingredientPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                    ingredientPanel.Children.Add(new TextBlock { Text = "Name:", Width = 50 });
                    ingredientPanel.Children.Add(new TextBox { Width = 100 });
                    ingredientPanel.Children.Add(new TextBlock { Text = "Quantity:", Width = 60, Margin = new Thickness(10, 0, 0, 0) });
                    ingredientPanel.Children.Add(new TextBox { Width = 50 });
                    ingredientPanel.Children.Add(new TextBlock { Text = "Unit:", Width = 30, Margin = new Thickness(10, 0, 0, 0) });
                    ingredientPanel.Children.Add(new TextBox { Width = 100 });
                    ingredientPanel.Children.Add(new TextBlock { Text = "Calories:", Width = 60, Margin = new Thickness(10, 0, 0, 0) });
                    ingredientPanel.Children.Add(new TextBox { Width = 50 });
                    ingredientPanel.Children.Add(new TextBlock { Text = "Food Group:", Width = 80, Margin = new Thickness(10, 0, 0, 0) });
                    ingredientPanel.Children.Add(new TextBox { Width = 100 });
                    IngredientsPanel.Items.Add(ingredientPanel);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number of ingredients.");
            }
        }

        // Adds step input fields based on the entered count
        private void AddSteps_Click(object sender, RoutedEventArgs e)
        {
            int count;
            if (int.TryParse(StepsCountTextBox.Text, out count) && count > 0)
            {
                StepsPanel.Items.Clear();
                for (int i = 0; i < count; i++)
                {
                    StackPanel stepPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                    stepPanel.Children.Add(new TextBlock { Text = $"Step {i + 1}:", Width = 50 });
                    stepPanel.Children.Add(new TextBox { Width = 300 });
                    StepsPanel.Items.Add(stepPanel);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number of steps.");
            }
        }

        // Displays the recipe details
        private void DisplayRecipe_Click(object sender, RoutedEventArgs e)
        {
            ReadIngredients();
            ReadSteps();
            DisplayRecipe();
        }

        // Scales the recipe quantities based on the selected factor
        private void ScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            double scale = Convert.ToDouble((sender as Button).Tag);
            currentRecipe.Scale(scale);
            DisplayRecipe();
        }

        // Resets ingredient quantities to original values
        private void ResetQuantities_Click(object sender, RoutedEventArgs e)
        {
            currentRecipe.Ingredients = new List<Ingredient>(originalIngredients);
            DisplayRecipe();
        }

        // Clears all input fields and the current recipe
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            IngredientsPanel.Items.Clear();
            StepsPanel.Items.Clear();
            RecipeOutputTextBlock.Text = "";
            TotalCaloriesTextBlock.Text = "";
            currentRecipe = new Recipe();
            originalIngredients.Clear();
        }

        // Adds a new recipe to the list and updates the display
        private void AddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(RecipeNameTextBox.Text))
            {
                ReadIngredients();
                ReadSteps();
                currentRecipe.Name = RecipeNameTextBox.Text;
                recipes.Add(currentRecipe);
                currentRecipe = new Recipe();
                originalIngredients.Clear();
                UpdateRecipeList();
                ClearAll_Click(sender, e);
                RecipeNameTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.");
            }
        }

        // Displays the selected recipe from the list
        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeListBox.SelectedItem != null)
            {
                currentRecipe = recipes.FirstOrDefault(r => r.Name == RecipeListBox.SelectedItem.ToString());
                if (currentRecipe != null)
                {
                    DisplayRecipe();
                }
            }
        }

        // Reads ingredient input fields and stores the data
        private void ReadIngredients()
        {
            currentRecipe.Ingredients.Clear();
            foreach (StackPanel panel in IngredientsPanel.Items)
            {
                var name = (panel.Children[1] as TextBox).Text;
                var quantity = Convert.ToDouble((panel.Children[3] as TextBox).Text);
                var unit = (panel.Children[5] as TextBox).Text;
                var calories = Convert.ToDouble((panel.Children[7] as TextBox).Text);
                var foodGroup = (panel.Children[9] as TextBox).Text;

                var ingredient = new Ingredient { Name = name, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup };
                currentRecipe.Ingredients.Add(ingredient);
            }

            originalIngredients = new List<Ingredient>(currentRecipe.Ingredients);
        }

        // Reads step input fields and stores the data
        private void ReadSteps()
        {
            currentRecipe.Steps.Clear();
            foreach (StackPanel panel in StepsPanel.Items)
            {
                var description = (panel.Children[1] as TextBox).Text;
                currentRecipe.Steps.Add(description);
            }
        }

        // Displays the current recipe details, including total calories
        private void DisplayRecipe()
        {
            RecipeOutputTextBlock.Text = currentRecipe.ToString();
            double totalCalories = currentRecipe.Ingredients.Sum(i => i.Calories * i.Quantity);
            TotalCaloriesTextBlock.Text = $"Total Calories: {totalCalories}";

            if (totalCalories > 300)
            {
                MessageBox.Show("Warning: Total calories exceed 300!");
            }
        }

        // Updates the list of recipes displayed in the ListBox
        private void UpdateRecipeList()
        {
            RecipeListBox.Items.Clear();
            foreach (var recipe in recipes.OrderBy(r => r.Name))
            {
                RecipeListBox.Items.Add(recipe.Name);
            }
        }
    }

    // Class representing an ingredient
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        public override string ToString()
        {
            return $"{Quantity} {Unit} of {Name} ({Calories} cal, {FoodGroup})";
        }
    }

    // Class representing a recipe
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

        // Scales the quantities of all ingredients
        public void Scale(double factor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        // Returns a string representation of the recipe
        public override string ToString()
        {
            var recipeString = $"Recipe: {Name}\n\nIngredients:\n";
            foreach (var ingredient in Ingredients)
            {
                recipeString += ingredient.ToString() + "\n";
            }

            recipeString += "\nSteps:\n";
            for (int i = 0; i < Steps.Count; i++)
            {
                recipeString += $"Step {i + 1}: {Steps[i]}\n";
            }

            return recipeString;
        }
    }
}
