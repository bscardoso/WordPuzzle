# WordPuzzle

Initially I tried some different approaches to solve the problem but the final solution I have chosen can be divided in 3 main steps:
1.	Get all the valid words (with 4 letters) from the dictionary and build up a structure containing all the possible and valid movements (changing only one letter for certain word, we could get to a list of other words).
2.	From the structure built up in step 1, we can now find all the possible ways to go from the start word to the end word and store those solutions in memory in addition to the solution depth.
3.	At this last stage, it gets only the solution with the smaller depth – so it will show the list of words that require less changes to go from the start word to the end word changing one letter at a time.
