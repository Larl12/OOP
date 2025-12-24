class Car:
    def __init__(self, brand, model, mileage=0):
    
        self.brand = brand      # марка
        self.model = model      # модель
        self.mileage = mileage  # пробег

    def drive(self, distance):
    
        self.mileage += distance

    def __str__(self):
    
        return f'{self.brand} {self.model} ({self.mileage} км)'

if __name__ == '__main__':
    my_car = Car('VAZ', '2110')
    print(my_car)              

    my_car.drive(100)          
    print(my_car)              

    my_car.drive(200)          
    print(my_car)              