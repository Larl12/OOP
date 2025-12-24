class Animal:
    def __init__(self, species, weight):
        self.species = species
        self.weight = weight

    def description(self):
        return f"Это {self.species}, массой {self.weight} кг."

class Dog(Animal):
    def __init__(self, species, weight, hair_length):
        super().__init__(species, weight)
        self.hair_length = hair_length

    def description(self):
        base_description = super().description()
        return f"{base_description} Его шерсть длиной {self.hair_length} см."

class Cat(Animal):
    def __init__(self, species, weight, color):
        super().__init__(species, weight)
        self.color = color

    def description(self):
        base_description = super().description()
        return f"{base_description} Окрас кота — {self.color}."

dog = Dog("Собака породы Такса", 30, 150)
cat = Cat("Кошка породы Манчикен", 4, "белый")

print(dog.description())  
print(cat.description())  