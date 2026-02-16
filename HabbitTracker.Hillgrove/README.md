## How the App Works

This app helps you track your daily habits. You can add, view, and manage your habits, making it easy to monitor your progress and stay consistent. All habit data is saved locally in a SQLite database for persistence.


## Though Process

really wasn't much to it as it was a paint-by-numbers tutorial. It was nice to re-learn ADO again as it's been some time since I've done anything DB related.

I did encounter a rather serious bug in the original repo / video, as methods can (potentially) recursively call eachother and fill up the stack, due to calling the "menu" class from the underlying methods, rather than returning from those methods.