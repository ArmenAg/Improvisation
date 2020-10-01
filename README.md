Automating Music Composition and Melody Generation.
==================================================

Many believe that the quintessence of human creativity is the aptitude to create art regardless of the art form itself. One of the more popular art forms, music,which has been viewed as an intellectual workout and composers such as Bach and Chopin are viewed as geniuses in our modern world. The intrinsic question to ask was, could software even begin to mimic the creativity of these composers and their compositions. 

This project succesfully answers that question.

Although the majority of this project is based on Music generation, the code is completly generic and can be applied to other fields such as auto generation of papers. 

For any questions or query email me at armen.ag@live.com

You can find the respective paper written at the root directory.

Thank you for reading.

How to use the Test Application
========================================
1. Download the project off of github.
2. Open the .sln file in visual studio.
3. Press Ctrl+F5 to launch the sample UI.
4. First we need to teach our program what is good music and what is bad. To do so, press on Create Neural Net.
5. Give it "good music" samples by pressing Load Good Midi Files and selecting desired midi files. I have included a folder with different types of music. You can, for example, select all eminem files to be represented as good.
6. Give it "bad music" samples by pressing Load Okay Midi Files. In the folder included, you could, for example, select ella fitzgerald files as bad.
7. If your music has a lot chords (such as in classical music), chord recognition will run for a long time,to decrease time allow the chord recognition to run note by note by checking the "Note by Note" checkbox. 
8. Press Begin training.
9. If wanted, save the file.
10. Now we need to create our statistical model (Markov graph), you can do so by selecting Create Stat Model.
11. Load up your musical samples and change the range of the size of the NGrams by shifting the two numbers at the bottom.
12. Again if your music is complicated, check the "Note by Note" checkbox.
13. Now everything is ready. Shift the variables to your liking and press "Start Music Generation".
14. Once the search is done you can play the various melodies by pressing on the items listed in the Listbox on the right. The general rule is higher on the Listbox = higher rated by a neural network.

Have Fun :)
