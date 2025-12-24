import functools

def log_call(func):
    @functools.wraps(func)
    def wrapper(*args, **kwargs):

        print(f"CALL: {func.__name__}({args}, {kwargs})")
        
        result = func(*args, **kwargs)
        
        print(f"RET: {result}")
        
        return result
    
    return wrapper

@log_call
def add(a, b):
    return a + b

print(add(2, 3))