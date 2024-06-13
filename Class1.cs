using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApplication
{
    // Define a delegate for notifying when a recipe exceeds 300 calories
    public delegate void RecipeCaloriesExceededHandler(string recipeName, int totalCalories);

    // Class representing a recipe
    internal class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Steps { get; set; } = new List<string>();

        // Event for notifying when a recipe exceeds 300 calories
        public event RecipeCaloriesExceededHandler CaloriesExceeded;

        // Method to calculate total calories of the recipe
        public int CalculateTotalCalories()
        {
            int totalCalories = Ingredients.Sum(ingredient => ingredient.Calories);
            return totalCalories;
        }

        // Method to add an ingredient to the recipe
        public void AddIngredient(string name, int quantity, string unitOfMeasurement, int calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient
            {
                Name = name,
                Quantity = quantity,
                UnitOfMeasurement = unitOfMeasurement,
                Calories = calories,
                FoodGroup = foodGroup
            });
        }

        // Method to display recipe details
        public void DisplayRecipeDetails()
        {
            Console.WriteLine($"Recipe Name: {Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Name} - {ingredient.Quantity} {ingredient.UnitOfMeasurement}");
            }
            Console.WriteLine("Steps:");
            foreach (var step in Steps)
            {
                Console.WriteLine(step);
            }
        }

        // Method to check if total calories exceed 300 and raise event if needed
        public void CheckCalories()
        {
            int totalCalories = CalculateTotalCalories();
            if (totalCalories > 300)
            {
                CaloriesExceeded?.Invoke(Name, totalCalories);
            }
        }
    }

    // Class representing an ingredient
    internal class Ingredient
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasurement { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    // Main program class
    internal class Program
    {
        private static List<Recipe> recipes = new List<Recipe>(); // Use a generic collection to store recipes

        // Main method
        static void Main(string[] args)
        {
            while (true)
            {
                DisplayMenu();
                int option = GetUserOption();
                ProcessOption(option);
            }
        }

        // Method to display menu options
        private static void DisplayMenu()
        {
            Console.WriteLine("1 - Enter recipe details");
            Console.WriteLine("2 - Display list of recipes");
            Console.WriteLine("3 - Display a recipe");
            Console.WriteLine("4 - Clear data");
        }

        // Method to get user option
        private static int GetUserOption()
        {
            Console.WriteLine("Enter your choice:");
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 4)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
            }
            return option;
        }

        // Method to process user option
        private static void ProcessOption(int option)
        {
            switch (option)
            {
                case 1:
                    EnterRecipeDetails();
                    break;
                case 2:
                    DisplayListOfRecipes();
                    break;
                case 3:
                    DisplayRecipe();
                    break;
                case 4:
                    ClearData();
                    break;
            }
        }

        // Method to enter recipe details
        private static void EnterRecipeDetails()
        {
            Recipe recipe = new Recipe();
            Console.WriteLine("Enter recipe name:");
            recipe.Name = Console.ReadLine();

            Console.WriteLine("Enter number of ingredients:");
            int ingreNum = int.Parse(Console.ReadLine());

            for (int i = 0; i < ingreNum; i++)
            {
                Console.WriteLine("Enter Ingredient Name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter ingredient quantity:");
                int quantity = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter unit of measurement:");
                string unitOfMeasurement = Console.ReadLine();

                Console.WriteLine("Enter number of calories:");
                int calories = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter food group:");
                string foodGroup = Console.ReadLine();

                recipe.AddIngredient(name, quantity, unitOfMeasurement, calories, foodGroup);
            }

            Console.WriteLine("Enter number of steps:");
            int stepNum = int.Parse(Console.ReadLine());

            for (int i = 0; i < stepNum; i++)
            {
                Console.WriteLine("Enter step description:");
                recipe.Steps.Add(Console.ReadLine());
            }

            // Subscribe to the event
            recipe.CaloriesExceeded += Recipe_CaloriesExceeded;

            // Add recipe to the list
            recipes.Add(recipe);

            // Check if total calories exceed 300
            recipe.CheckCalories();

            Console.WriteLine("Recipe added successfully.");
        }

        // Event handler for when recipe exceeds 300 calories
        private static void Recipe_CaloriesExceeded(string recipeName, int totalCalories)
        {
            Console.WriteLine($"Recipe '{recipeName}' exceeds 300 calories. Total calories: {totalCalories}");
        }

        // Method to display list of recipes
        private static void DisplayListOfRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
            }
            else
            {
                Console.WriteLine("List of Recipes:");
                foreach (var recipe in recipes.OrderBy(r => r.Name))
                {
                    Console.WriteLine(recipe.Name);
                }
            }
        }

        // Method to display a recipe
        private static void DisplayRecipe()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
            }
            else
            {
                Console.WriteLine("Enter recipe name to display:");
                string recipeName = Console.ReadLine();
                Recipe recipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
                if (recipe != null)
                {
                    recipe.DisplayRecipeDetails();
                }
                else
                {
                    Console.WriteLine("Recipe not found.");
                }
            }
        }

        // Method to clear data
        private static void ClearData()
        {
            recipes.Clear();
            Console.WriteLine("All data cleared successfully.");
        }
    }
}
