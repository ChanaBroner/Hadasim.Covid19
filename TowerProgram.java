import java.util.Scanner;
public class TowerProgram {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);

        while (true) {
            System.out.println("Please enter 1 or 2 or 3." +
                "\n1 for a rectangular tower, 2 for a triangular tower and 3 to exit the program:");
            int choice = scanner.nextInt();

            if (choice == 3)
                break;

            System.out.println("Please enter the height of the tower:");
            int height = scanner.nextInt();
            System.out.println("Please enter the width of the tower:");
            int width = scanner.nextInt();

            if (height < 2 || width < 2) {
                System.out.println("Invalid dimensions. Height and width must be at least 2.");
                return;
            }

            // Creating appropriate shape object based on user input
            Shape shape;
            if (choice == 1) {
                shape = new Rectangle(height, width);
            } else if (choice == 2) {
                shape = new Triangle(height, width);
            } else {
                System.out.println("Invalid selection. Please choose 1, 2, or 3.");
                continue;
            }

            // Calculating or printing based on shape type
            if (shape instanceof Rectangle) {
                ((Rectangle) shape).calculate();
            } else if (shape instanceof Triangle) {
                System.out.println("Please choose an option for triangular tower:" +
                    "\n1. Calculation of the perimeter of the triangle" +
                    "\n2. Printing the triangle");
                int option = scanner.nextInt();
                if (option == 1) {
                    System.out.println("The perimeter of the triangle: " + ((Triangle) shape).calculatePerimeter());
                } else if (option == 2) {
                    ((Triangle) shape).printTriangle();
                } else {
                    System.out.println("Invalid option. Please choose 1 or 2.");
                }
            }
        }

        scanner.close();
    }
}