// See https://aka.ms/new-console-template for more information


using System;

namespace ASCIIShapes
{

    public abstract class Shape
    {
        const int minHeight = 2;

        private int oHeight;

        private int oWidth;

        private string shapeName;
        public string ShapeName { get; set; }

        private char printCharacter = '*';

        private char backgroundCharacter = ' ';

        public char PrintCharacter
        {
            get
            { return printCharacter; }
            set
            { printCharacter = value; }
        }

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

        protected char[,] oArray;

        public char[,] GetArray
        { get { return oArray; } }

        public void Print()
        {
            for (int i = 0; i < ObjectHeight; i++)
            {
                for (int j = 0; j < ObjectWidth; j++)
                {
                    Console.Write(oArray[i, j]);
                    if (this.ShapeName != "ACUTE ISOSCLES TRIANGLE")
                    {
                        Console.Write(' ');
                    }
                    
                }

                Console.WriteLine();
            }
        }


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

        public int MaxLabelLength(int row)
        {
            int rowIndex = row - 1;

            int maxLabel = (LastIndexSearch(rowIndex) - FirstIndexSearch(rowIndex)) - 1;

            return maxLabel;
        }

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


        protected bool LabelSizeCheck(int row, int length)
        {

            if (length > MaxLabelLength(row))
            {
                return false;
            }
            else
            {
                return true;
            }

        }


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


    public class IsosclesTriangle : Shape
    {

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

        private void LoadArray()
        {
            for (int a = 0; a < ObjectHeight; a++)
            {

                //inner loop to write values to array
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

        private void LoadArray()
        {
            for (int a = 0; a < ObjectHeight; a++)
            {

                //inner loop to write values to array
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

                //inner loop to write values to array
                for (int b = 0; b < ObjectWidth; b++)
                {
                    oArray[a, b] = PrintCharacter;
                }
            }
        }


    }



    public class Diamond : Shape
    {

        private const int minHeight = 3;

        public Diamond(int height)
        {

            ShapeName = "DIAMOND";

            ObjectHeight = height;

            if (height < minHeight)
            {
                throw new ArgumentOutOfRangeException("height");
            }
            else if (height % 2 == 0)
            {
                EvenDiamond(height);
            }
            else
            {
                OddDiamond(height);
            }
        }

        private void EvenDiamond(int height)
        {

            IsosclesTriangle topTri = new IsosclesTriangle(height / 2);

            IsosclesTriangle botTri = new IsosclesTriangle(height / 2);

            botTri.Invert();

            LoadArray(topTri, botTri, true);

        }

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



    public static class Menu
    {

        private static bool continueProgram = true;

        private static bool continueShapeSubmenu = true;

        private static bool continueHeightSubmenu = true;

        private static bool continueWidthSubmenu = true;

        private static bool continueLabelSubmenu = true;

        private static string[] shapeOptions = { "Square", "Rectangle", "Triangle", "Diamond" };

        private static string[] triOptions = { "Acute Isosles Triangle", "Right Isoscles Triangle" };

        private static int dimHeight = 10;
        private static int dimWidth = 10;

        private static void ResetMenuFlags()
        {
            //reset the menu flags
            continueProgram = true;
            continueShapeSubmenu = true;
            continueHeightSubmenu = true;
            continueWidthSubmenu = true;
            continueLabelSubmenu = true;
        }

        private static void ReturnToMainMenu()
        {
            continueProgram = true;
            continueShapeSubmenu = false;
            continueHeightSubmenu = false;
            continueWidthSubmenu = false;
            continueLabelSubmenu = false;
        }


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

                if (shape == "RECTANGLE")
                {
                    WidthSubmenu(dimHeight);
                }
                else
                {
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
                    Console.WriteLine("Invalid input. Please enter a positive integer between 2 and 50 or a menu option.");
                }
                else
                {
                    Console.WriteLine("Please choose a width for this shape.");

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
                    continueHeightSubmenu = false;
                    continueShapeSubmenu = false;
                    continueProgram = false;
                    continue;
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


        private static void LabelSubmenu(Shape inputShape)
        {

            int errorCode = 0;

            string labelChoice = "";

            string label = "";

            while (continueLabelSubmenu)
            {

                ResetMenuFlags();

                ScreenBanner("LABEL SELECTION: " + inputShape.ShapeName);
                Console.WriteLine();
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
                    Console.WriteLine("Do you want to print a custom label on the shape?");
                }
                

                errorCode = 0;

                Console.WriteLine();
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


                Menu.HomeScreen();


            }
        }
    }
}
