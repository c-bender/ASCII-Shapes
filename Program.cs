//  Author: C.Bender
// Project: ASCII Shapes Problem
//Comments: Incorporated all problem requirements and added several additional features and UI elements.


using System;

namespace ASCIIShapes
{
    //parent class for all shape objects
    public abstract class Shape
    {
        const int minHeight = 2;

        private int oHeight;

        private int oWidth;

        private string shapeName;
        public string ShapeName { get; set; }

        private char printCharacter = '*';

        private char backgroundCharacter = ' ';

        //enables future addition of user control over print character
        public char PrintCharacter
        {
            get
            { return printCharacter; }
            set
            { printCharacter = value; }
        }

        //enables future addition of user control over background character
        public char BackgroundCharacter
        {
            get
            { return backgroundCharacter; }
            set
            { backgroundCharacter = value; }
        }

        //property to allow data validation before writing the backing field
        public int ObjectHeight
        {
            get
            {
                return oHeight;
            }

            set
            {
                if (value < minHeight)
                    throw new ArgumentOutOfRangeException("value");
                else
                    oHeight = value;
            }

        }

        public int ObjectWidth
        {
            get { return oWidth; }
            set { oWidth = value; }
        }

        //two-dimensional array backing field to store the ascii shapes
        //all shapes created and stored as rectangular arrays of print character and whitespace (background character)
        protected char[,] oArray;


        public char[,] GetArray
        { get { return oArray; } }

        //generic print method for all shapes
        public void Print()
        {
            for (int i = 0; i < ObjectHeight; i++)
            {
                for (int j = 0; j < ObjectWidth; j++)
                {
                    Console.Write(oArray[i, j]);

                    //console font height is 2x font width, so whitespace needs to be added between every x-coordinate for the aspect ratio
                    //the acute isoscles triangle doesn't need this whitespace
                    if (this.ShapeName != "ACUTE ISOSCLES TRIANGLE")
                    {
                        Console.Write(' ');
                    }
                    
                }

                Console.WriteLine();
            }
        }

        //get the x-index (column) of the left edge of the shape
        protected int FirstIndexSearch(int rowIndex)
        {
            for (int i = 0; i < ObjectWidth; i++)
            {
                if (oArray[rowIndex, i] == PrintCharacter)
                {
                    return i;
                }
                else
                {
                    continue;
                }
            }

            return -1;

        }

        //get the x-index (column of the right edge of the shape
        protected int LastIndexSearch(int rowIndex)
        {
            for (int i = ObjectWidth - 1; i > -1; i--)
            {
                if (oArray[rowIndex, i] == PrintCharacter)
                {
                    return i;
                }
                else
                {
                    continue;
                }
            }

            return -1;

        }

        //calculate the maximum label length for a given row
        //labels are precluded from including a perimeter pixel and must be wholly contained within the shape
        public int MaxLabelLength(int row)
        {
            int rowIndex = row - 1;

            int maxLabel = (LastIndexSearch(rowIndex) - FirstIndexSearch(rowIndex)) - 1;

            return maxLabel;
        }

        //calculate the maximum label length for a ANY row in the shape
        //labels are precluded from including a perimeter pixel and must be wholly contained within the shape
        public int GetMaxLabelLength()
        {
            int maxLength = 0;

            for (int i = 0; i < ObjectHeight; i++)
            {
                if (MaxLabelLength(i + 1) > maxLength)
                {
                    maxLength = MaxLabelLength(i + 1);
                }
            }

            return maxLength;
        }

        //calculate for a given label which rows it could validly be printed within
        public List<string> GetLabelRowOptions(string label)
        {

            List<string> optionsList = new List<string>();

            for (int i = 0; i < ObjectHeight; i++)
            {
                if (label.Length > MaxLabelLength(i + 1))
                {
                    continue;
                }
                else
                {
                    int rowValue = i + 1;
                    optionsList.Add(rowValue.ToString());
                }
            }

            return optionsList;
        }

        //functionally a submenu to receive and validate label data from the user
        public void AttachLabelDialogue(string label)
        {
            List<string> optionsList = GetLabelRowOptions(label);

            bool continueLabelLoop = true;
            string rowChoice = "";
            string spacer = "";

            if (optionsList.Count > 1)
            {
                spacer = ", ";
            }

            while (continueLabelLoop)
            {
                Console.WriteLine();
                Console.Write("Row Options for the Selected (or Default) Label Size: ");
                for (int i = 0; i < optionsList.Count; i++)
                {
                    if (i < optionsList.Count - 1)
                    {
                        Console.Write(optionsList[i] + spacer);
                    }
                    else
                    {
                        Console.Write(optionsList[i]);
                    }
                }
                Console.WriteLine();
                Console.Write("Please choose a row for the label (Default = 4 [if applicable]; Secondary Default = First Available Row): ");
                
                rowChoice = Console.ReadLine();

                if (rowChoice == "")
                {
                    if (optionsList.Contains("4"))
                    {
                        rowChoice = "4";
                    }
                    else
                    {
                        rowChoice = optionsList[0];
                    }
                }

                if (optionsList.Contains(rowChoice))
                {
                    LabelAssign(Int32.Parse(rowChoice), label);
                    continueLabelLoop = false;
                }
                else
                {
                    continue;
                }

            }
        }


        //replace the print characters in the shape with the characters from the label string
        protected void LabelAssign(int row, string label)
        {

            char[] chars = label.ToCharArray();

            int rowIndex = row - 1;

            int shapeStart = FirstIndexSearch(rowIndex);

            int shapeEnd = LastIndexSearch(rowIndex);

            int maxLabel = (shapeEnd - shapeStart) - 1;

            int labelStart = (shapeStart + ((maxLabel - chars.Length) / 2)) + 1;

            int counter = 0;

            for (int i = labelStart; i < labelStart + chars.Length; i++)
            {
                this.oArray[rowIndex, i] = chars[counter];

                counter++;

            }

        }



        //built for future functionality to allow shape transformations (mirror, rotate, etc.)
        public void Mirror()
        {

            char[,] outputArray = new char[ObjectHeight, ObjectWidth];

            for (int a = 0; a < ObjectHeight; a++)
            {
                int b = 0;

                for (int j = ObjectWidth - 1; j > -1; j--)
                {
                    outputArray[a, b] = this.oArray[a, j];
                    b++;
                }
            }

            this.oArray = outputArray;
        }


        //invert the rows of a shape
        //used by the Diamond class to construct diamonds out of two triangles
        public void Invert()
        {
            char[,] outputArray = new char[ObjectHeight, ObjectWidth];

            for (int b = 0; b < ObjectWidth; b++)
            {
                int a = 0;

                for (int j = ObjectHeight - 1; j > -1; j--)
                {
                    outputArray[a, b] = this.oArray[j, b];
                    a++;
                }
            }

            this.oArray = outputArray;
        }


    }

    //child class for acute isoscles triangles
    public class IsosclesTriangle : Shape
    {
        //index of the midpoint of the triangle
        private int midPointIndex;

        public IsosclesTriangle(int height)
        {

            ShapeName = "ACUTE ISOSCLES TRIANGLE";

            ObjectHeight = height;

            ObjectWidth = (ObjectHeight * 2) - 1;

            midPointIndex = ((ObjectWidth + 1) / 2) - 1;

            oArray = new char[ObjectHeight, ObjectWidth];

            LoadArray();

        }

        //load the array with the shape
        private void LoadArray()
        {
            //outer loop steps through rows
            for (int a = 0; a < ObjectHeight; a++)
            {

                //inner loop steps through columns
                for (int b = 0; b < ObjectWidth; b++)
                {
                    if (b >= midPointIndex - a && b <= midPointIndex + a)
                    {
                        oArray[a, b] = PrintCharacter;
                    }
                    else
                    {
                        oArray[a, b] = BackgroundCharacter;
                    }

                }
            }

        }


    }

    //child class for right triangles
    public class RightTriangle : Shape
    {

        public RightTriangle(int height)
        {

            ShapeName = "RIGHT ISOSCLES TRIANGLE";

            ObjectHeight = height;

            ObjectWidth = ObjectHeight;

            oArray = new char[ObjectHeight, ObjectWidth];

            LoadArray();

        }

        //triangle will be left-aligned by default
        //Mirror() method can be called if right-alignment is needed
        private void LoadArray()
        {
            for (int a = 0; a < ObjectHeight; a++)
            {

                
                for (int b = 0; b < ObjectWidth; b++)
                {
                    if (b <= a)
                    {
                        oArray[a, b] = PrintCharacter;
                    }
                    else
                    {
                        oArray[a, b] = BackgroundCharacter;
                    }

                }
            }

        }



    }

    //child class for rectangles and squares (a square is a rectangle with equal length sides)
    public class Rectangle : Shape
    {

        public Rectangle(int height, int width)
        {
            ShapeName = "RECTANGLE";

            ObjectHeight = height;

            ObjectWidth = width;

            oArray = new char[ObjectHeight, ObjectWidth];

            LoadArray();

        }

        //overloaded method for making squares
        public Rectangle(int height)
        {
            ShapeName = "SQUARE";

            ObjectHeight = height;

            ObjectWidth = height;

            oArray = new char[ObjectHeight, ObjectWidth];

            LoadArray();

        }

        private void LoadArray()
        {
            for (int a = 0; a < ObjectHeight; a++)
            {
                
                for (int b = 0; b < ObjectWidth; b++)
                {
                    oArray[a, b] = PrintCharacter;
                }
            }
        }


    }


    //child class for diamond shapes
    public class Diamond : Shape
    {
        //diamonds have to be 3 high or else they're triangles
        private const int minHeight = 3;

        public Diamond(int height)
        {

            ShapeName = "DIAMOND";

            ObjectHeight = height;

            //if the height is even, two triangles of equal size build it
            //if the height is odd, one triangle needs to be smaller by 1
            if (height % 2 == 0)
            {
                EvenDiamond(height);
            }
            else
            {
                OddDiamond(height);
            }
        }

        //build diamonds of even height
        private void EvenDiamond(int height)
        {

            IsosclesTriangle topTri = new IsosclesTriangle(height / 2);

            IsosclesTriangle botTri = new IsosclesTriangle(height / 2);

            botTri.Invert();

            LoadArray(topTri, botTri, true);

        }

        //build diamonds of odd height
        private void OddDiamond(int height)
        {

            IsosclesTriangle topTri = new IsosclesTriangle((height + 1) / 2);

            IsosclesTriangle botTri = new IsosclesTriangle((height + 1) / 2);

            botTri.Invert();

            LoadArray(topTri, botTri, false);

        }

        private void LoadArray(IsosclesTriangle topTri, IsosclesTriangle botTri, bool isEven)
        {
            ObjectWidth = topTri.ObjectWidth;

            oArray = new char[ObjectHeight, ObjectWidth];

            if (isEven)
            {

                for (int a = 0; a < ObjectHeight; a++)
                {

                    if (a < (ObjectHeight + 1) / 2)
                    {
                        for (int b = 0; b < ObjectWidth; b++)
                        {
                            oArray[a, b] = topTri.GetArray[a, b];
                        }
                    }
                    else
                    {
                        for (int b = 0; b < ObjectWidth; b++)
                        {
                            oArray[a, b] = botTri.GetArray[a - (ObjectHeight / 2), b];
                        }
                    }



                }
            }
            else
            {
                for (int a = 0; a < ObjectHeight; a++)
                {

                    if (a < (ObjectHeight + 1) / 2)
                    {
                        for (int b = 0; b < ObjectWidth; b++)
                        {
                            oArray[a, b] = topTri.GetArray[a, b];
                        }
                    }
                    else
                    {
                        for (int b = 0; b < ObjectWidth; b++)
                        {
                            oArray[a, b] = botTri.GetArray[a - ((ObjectHeight) / 2), b];
                        }
                    }
                }
            }



        }

    }


    //static menu class for the majority of user input, UI logic, and program state
    public static class Menu
    {
        //set of flags that determine which menu to return to after relevant jump statements
        private static bool continueProgram = true;
        private static bool continueShapeSubmenu = true;
        private static bool continueHeightSubmenu = true;
        private static bool continueWidthSubmenu = true;
        private static bool continueLabelSubmenu = true;

        //static options to build menu text
        private static string[] shapeOptions = { "Square", "Rectangle", "Triangle", "Diamond" };
        private static string[] triOptions = { "Acute Isosles Triangle", "Right Isoscles Triangle" };

        //defaults for height and width inputs to shape object constructors
        private static int dimHeight = 10;
        private static int dimWidth = 10;

        //resets the menu flags after an error is caught and handled
        private static void ResetMenuFlags()
        {
            //reset the menu flags
            continueProgram = true;
            continueShapeSubmenu = true;
            continueHeightSubmenu = true;
            continueWidthSubmenu = true;
            continueLabelSubmenu = true;
        }

        //sets control flags to a state that returns user to main menu after a jump statement
        private static void ReturnToMainMenu()
        {
            continueProgram = true;
            continueShapeSubmenu = false;
            continueHeightSubmenu = false;
            continueWidthSubmenu = false;
            continueLabelSubmenu = false;
        }

        //prints the screen banner for each menu / program state
        public static void ScreenBanner(string bannerText)
        {
            Console.Clear();
            Console.WriteLine(bannerText);
            for (int i = 0; i < bannerText.Length; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        //main menu
        public static void HomeScreen()
        {

            bool errorEntry = false;

            string shapeChoice = "";

            while (continueProgram)
            {

                ResetMenuFlags();

                ScreenBanner("ASCII SHAPE GENERATOR");

                for (int i = 0; i < shapeOptions.Length; i++)
                {
                    Console.WriteLine((i + 1) + " - " + shapeOptions[i]);
                }
                Console.WriteLine();
                Console.WriteLine("X - Exit Program");
                Console.WriteLine();
                if (errorEntry)
                {
                    Console.Write("Previous selection \"" + shapeChoice + "\" was invalid. Please enter a choice from the menu options: ");
                }
                else
                {
                    Console.Write("Please Choose an Option: ");
                }

                errorEntry = false;

                shapeChoice = Console.ReadLine();

                shapeChoice = shapeChoice.ToUpper();

                //using a switch with a default that sets an error flag in order to catch and handle many types of incorrect input
                switch (shapeChoice)
                {
                    case "1":
                        HeightSubmenu("SQUARE");
                        continue;
                    case "2":
                        HeightSubmenu("RECTANGLE");
                        continue;
                    case "3":
                        Console.Clear();
                        TriangleSubmenu();
                        continue;
                    case "4":
                        HeightSubmenu("DIAMOND");
                        continue;
                    case "X":
                        continueProgram = false;
                        continue;
                    default:
                        errorEntry = true;
                        continue;

                }
            }

        }

        //submenu for building triangles
        public static void TriangleSubmenu()
        {

            bool errorEntry = false;

            string triChoice = "";

            while (continueShapeSubmenu)
            {

                ResetMenuFlags();

                ScreenBanner("TRIANGLE TYPE");
                for (int i = 0; i < triOptions.Length; i++)
                {
                    Console.WriteLine((i + 1) + " - " + triOptions[i]);
                }
                Console.WriteLine();
                Console.WriteLine("M - Return to Main Menu");
                Console.WriteLine("X - Exit Program");
                Console.WriteLine();
                if (errorEntry)
                {
                    Console.Write("Previous selection \"" + triChoice + "\" was invalid. Please enter a choice from the menu options: ");
                }
                else
                {
                    Console.Write("Please Choose an Option: ");
                }

                errorEntry = false;
                triChoice = Console.ReadLine();
                triChoice = triChoice.ToUpper();

                switch (triChoice)
                {
                    case "1":
                        HeightSubmenu("ACUTE ISOSCLES TRIANGLE");
                        continue;
                    case "2":
                        HeightSubmenu("RIGHT ISOSCLES TRIANGLE");
                        continue;
                    case "M":
                        continueShapeSubmenu = false;
                        continue;
                    case "X":
                        continueProgram = false;
                        continueShapeSubmenu = false;
                        continue;
                    default:
                        errorEntry = true;
                        continue;

                }
            }

        }

        //submenu for choosing the height of the shape
        public static void HeightSubmenu(string shape)
        {

            bool errorEntry = false;
            bool diamondError = false;
            string heightChoice = "";

            shape = shape.ToUpper();

            while (continueHeightSubmenu)
            {

                ResetMenuFlags();

                ScreenBanner("HEIGHT SELECTION: " + shape);

                //additional logic needed to check the minimum heights of different shapes
                if ((errorEntry || diamondError) && shape == "DIAMOND")
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer between 3 and 50 for a diamond shape or a menu option. (Default: 10)");
                }
                else if (errorEntry)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer between 2 and 50 or a menu option. (Default: 10)");
                }
                else
                {
                    Console.WriteLine("Please choose a height for this shape. (Default: 10)");

                }

                errorEntry = false;
                diamondError = false;
                Console.WriteLine();

                //only show the option to return to the triangle submenu if the user has chosen a triangle
                if (shape == "ACUTE ISOSCLES TRIANGLE" || shape == "RIGHT ISOSCLES TRIANGLE")
                {
                    Console.WriteLine("S - Return to Triangle Submenu");
                }
                Console.WriteLine("M - Return to Main Menu");
                Console.WriteLine("X - Exit Program");
                Console.WriteLine();
                if (shape == "DIAMOND")
                {
                    Console.Write("INPUT (3-50): ");
                }
                else
                {
                    Console.Write("INPUT (2-50): ");
                }


                heightChoice = Console.ReadLine();

                heightChoice = heightChoice.ToUpper();

                //only return to the triangle submenu if the user has chosen a triangle
                if (heightChoice == "S" && (shape == "ACUTE ISOSCLES TRIANGLE" || shape == "RIGHT ISOSCLES TRIANGLE"))
                {
                    continueHeightSubmenu = false;
                    continue;
                }
                else if (heightChoice == "M")
                {
                    continueHeightSubmenu = false;
                    continueShapeSubmenu = false;
                    continue;
                }
                else if (heightChoice == "X")
                {
                    continueHeightSubmenu = false;
                    continueShapeSubmenu = false;
                    continueProgram = false;
                    continue;
                }
                else if (heightChoice == "")
                {
                    heightChoice = "10";
                }
                else
                {
                    //if user did not chose a menu option, check if they submitted an INT
                    try
                    {
                        dimHeight = Int32.Parse(heightChoice);
                    }
                    catch (FormatException)
                    {
                        errorEntry = true;
                        continue;
                    }
                }

                //height "out of range" logic
                if (shape == "DIAMOND" && (dimHeight < 3 || dimHeight > 50))
                {
                    diamondError = true;
                    continue;
                }

                if (dimHeight < 2 || dimHeight > 50)
                {
                    errorEntry = true;
                    continue;
                }

                //if the shape is a rectangle, route through the "Width" submenu first
                if (shape == "RECTANGLE")
                {
                    WidthSubmenu(dimHeight);
                }
                else
                {
                    //build the objects and pass them to the label submenu
                    if (shape == "SQUARE")
                    {
                        Rectangle inputShape = new Rectangle(dimHeight);
                        LabelSubmenu(inputShape);
                    }
                    else if (shape == "ACUTE ISOSCLES TRIANGLE")
                    {
                        IsosclesTriangle inputShape = new IsosclesTriangle(dimHeight);
                        LabelSubmenu(inputShape);
                    }
                    else if (shape == "RIGHT ISOSCLES TRIANGLE")
                    {
                        RightTriangle inputShape = new RightTriangle(dimHeight);
                        LabelSubmenu(inputShape);
                    }
                    else if (shape == "DIAMOND")
                    {
                        Diamond inputShape = new Diamond(dimHeight);
                        LabelSubmenu(inputShape);
                    }


                }


            }



        }

        //width submenu for rectangles
        private static void WidthSubmenu(int height)
        {
            bool errorEntry = false;
            string widthChoice = "";
            dimHeight = height;

            while (continueWidthSubmenu)
            {

                ResetMenuFlags();

                ScreenBanner("WIDTH SELECTION: RECTANGLE (Height: " + height + ")");

                if (errorEntry)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer between 2 and 50 (Default: 20) or a menu option.");
                }
                else
                {
                    Console.WriteLine("Please choose a width for this shape (Default: 20).");

                }

                errorEntry = false;
                Console.WriteLine();
                Console.WriteLine("H - Return to Height Menu");
                Console.WriteLine("M - Return to Main Menu");
                Console.WriteLine("X - Exit Program");
                Console.WriteLine();
                Console.Write("INPUT: ");

                widthChoice = Console.ReadLine();

                widthChoice = widthChoice.ToUpper();

                if (widthChoice == "H")
                {
                    continueWidthSubmenu = false;
                    continue;
                }
                else if (widthChoice == "M")
                {
                    continueHeightSubmenu = false;
                    continueWidthSubmenu = false;
                    continue;
                }
                else if (widthChoice == "X")
                {
                    continueWidthSubmenu= false;
                    continueHeightSubmenu = false;
                    continueShapeSubmenu = false;
                    continueProgram = false;
                    continue;
                }
                else if (widthChoice == "")
                {
                    dimWidth = 20;
                }
                else
                {
                    try
                    {
                        dimWidth = Int32.Parse(widthChoice);
                    }
                    catch (FormatException)
                    {
                        errorEntry = true;
                        continue;
                    }
                }

                if (dimWidth < 2 || dimWidth > 50)
                {
                    errorEntry = true;
                    continue;
                }


                Rectangle inputShape = new Rectangle(dimHeight, dimWidth);


                LabelSubmenu(inputShape);


            }
        }

        //submenu for attaching labels; passes control off to methods in the Shape class for final processing
        private static void LabelSubmenu(Shape inputShape)
        {

            int errorCode = 0;

            string labelChoice = "";

            string label = "";

            while (continueLabelSubmenu)
            {

                ResetMenuFlags();

                ScreenBanner("LABEL SELECTION: " + inputShape.ShapeName + " (Height: " + inputShape.ObjectHeight + ")");
                Console.WriteLine();

                //logic to redirect user after different types of incorrect input
                if (errorCode == 1)
                {
                    Console.WriteLine("Invalid input. Do you want to print a custom label on the shape (Default: LU)?");
                }
                else if (errorCode == 2)
                {
                    Console.WriteLine("Invalid label length. Do you want to print a custom label on the shape (Default: LU)?");
                }
                else if (errorCode == 3)
                {
                    Console.WriteLine("Current shape is not large enough to accept a label.");
                    Console.WriteLine("All labels must be attached fully within the perimeter of the shape.");
                    Console.WriteLine("Press any key to draw the shape with no label...");
                    Console.ReadKey();
                    Console.Clear();
                    inputShape.Print();
                    Console.WriteLine();
                    Console.WriteLine();
                    ReturnToMainMenu();
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
                }
                else if (errorCode == 4)
                {
                    Console.WriteLine("Current shape is not large enough to accept the default label (\"LU\").");
                    Console.WriteLine("All labels must be attached fully within the perimeter of the shape.");
                    Console.WriteLine("Press any key to draw the shape with no label...");
                    Console.ReadKey();
                    Console.Clear();
                    inputShape.Print();
                    Console.WriteLine();
                    Console.WriteLine();
                    ReturnToMainMenu();
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Do you want to print a custom label on the shape (Default: LU)?");
                }
                

                errorCode = 0;

                Console.WriteLine();
                Console.WriteLine("Y - Yes");
                Console.WriteLine("N - No");
                if (inputShape.ShapeName == "RECTANGLE")
                {
                    Console.WriteLine("W - Return to Width Menu");
                }
                Console.WriteLine("H - Return to Height Menu");
                if (inputShape.ShapeName == "ACUTE ISOSCLES TRIANGLE" || inputShape.ShapeName == "RIGHT ISOSCLES TRIANGLE")
                {
                    Console.WriteLine("T - Return to Triangle Menu");
                }
                Console.WriteLine("M - Return to Main Menu");
                Console.WriteLine("X - Exit Program");
                Console.WriteLine();
                Console.Write("INPUT (Y/N): ");

                labelChoice = Console.ReadLine();

                labelChoice = labelChoice.ToUpper();

                int maxLength = 0;

                switch (labelChoice)
                {
                    case "W":
                        continueLabelSubmenu = false;
                        continue;
                    case "H":
                        continueLabelSubmenu = false;
                        continue;
                    case "T":
                        continueLabelSubmenu = false;
                        continueHeightSubmenu = false;
                        continue;
                    case "M":
                        continueLabelSubmenu = false;
                        continueHeightSubmenu = false;
                        continue;
                    case "X":
                        continueLabelSubmenu = false;
                        continueHeightSubmenu = false;
                        continueWidthSubmenu = false;
                        continueShapeSubmenu = false;
                        continueProgram = false;
                        continue;
                    case "N":
                        maxLength = inputShape.GetMaxLabelLength();
                        if (maxLength < 2)
                        {
                            errorCode = 4;
                        }
                        else
                        {
                            label = "LU";
                            inputShape.AttachLabelDialogue(label);
                            Console.Clear();
                            inputShape.Print();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            ReturnToMainMenu();
                        }
                        continue;
                    case "Y":
                        Console.WriteLine();

                        maxLength = inputShape.GetMaxLabelLength();

                        if (maxLength < 1)
                        {
                            errorCode = 3;
                            continue;
                        }

                        Console.Write("Type a custom label here (Max Length for this Shape = " + maxLength + "): ");
                        
                        label = Console.ReadLine();


                        if (label.Length > maxLength)
                        {
                            errorCode = 2;
                            continue;
                        }

                        inputShape.AttachLabelDialogue(label);
                        Console.Clear();
                        inputShape.Print();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Press any key to return to the main menu...");
                        Console.ReadKey();
                        ReturnToMainMenu();

                        continue;

                    default:
                        errorCode = 1;
                        continue;

                }


                

            }

        }






        internal class Program
        {
            static void Main(string[] args)
            {

                //everything branches off of the HomeScreen method
                Menu.HomeScreen();


            }
        }
    }
}
