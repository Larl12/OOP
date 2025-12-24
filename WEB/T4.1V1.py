import functools

def log_with(level="INFO", prefix=""):
    def decorator(func):
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            call_message = f"{level} {prefix}{func.__name__}({args}, {kwargs})"
            print(call_message)
            
            result = func(*args, **kwargs)
            
            ret_message = f"RET: {result}"
            print(ret_message)
            
            return result
        
        return wrapper
    return decorator

@log_with(level="DEBUG", prefix="[calc] ")
def add(a, b):
    return a + b

print(add(2, b=3))