class Triangle extends Shape {
    public Triangle(int length, int width) {
        super(length, width);
    }

    // Method to calculate perimeter of the triangle
    public double calculatePerimeter() {
        double base = Math.sqrt(4 * width * width - 4 * length * length);
        return 2 * width + base;
    }

    // Method to print the triangle
    public void printTriangle() {
        // Check if the triangle can be printed
        if (width % 2 == 0 || width > 2 * length || length < 2 || width < 3) {
            System.out.println("The triangle cannot be printed :(");
            return;
        }
        int spaces = (width - 2) / 2 + 1;
        int numStars = 1;
        printRow(spaces, 1, 1);

        if (width == 3) {
            numStars += 2;
            spaces--;
            printRow(spaces, numStars, (length - 1));
        } else {
            int num_middleRows = (width - 2) / 2;
            int time_middleRows = (length - 2) / num_middleRows;
            int remainder = (length - 2) % num_middleRows;
            // Calculate the number of stars for the top part of the triangle

            // Print top part of the triangle
            while ((numStars + 2) < width) {
                numStars += 2;
                spaces--;
                if (remainder > 0) {
                    printRow(spaces, numStars, time_middleRows + 1);
                    remainder--;
                } else {
                    printRow(spaces, numStars, time_middleRows);
                }
            }

            // Print bottom part of the triangle
            printRow(0, width, 1);
        }
    }

    private void printRow(int spaces, int numStars, int time_middleRows) {
        for (int i = 0; i < time_middleRows; i++) {
            for (int j = 0; j < spaces; j++) {
                System.out.print(" ");
            }
            for (int j = 0; j < numStars; j++) {
                System.out.print("*");
            }
            for (int j = 0; j < spaces; j++) {
                System.out.print(" ");
            }
            System.out.println();
        }
    }
}