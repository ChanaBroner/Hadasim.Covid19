class Rectangle extends Shape {
    public Rectangle(int length, int width) {
        super(length, width);
    }

    // Method to calculate area or perimeter
    public void calculate() {
        if (length == width || Math.abs(width - length) > 5)
            System.out.println("Area of the rectangle: " + (width * length));
        else
            System.out.println("Perimeter of the rectangle: " + (2 * width + 2 * length));
    }
}