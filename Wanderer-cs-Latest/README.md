# Full Week Project: Wanderer - The RPG game

Build a hero based walking on tiles and killing monsters type of game. The hero
is controlled in a maze using the keyboard. Heroes and monsters have levels and
stats depending on their levels. The goal is reach the highest level by killing
the monsters holding the keys to the next level.

## Why?

The main goal of the project is to practice object oriented thinking. The best
way to practice it is: to create a bigger application and think about it's
architecture. It's one of the first occasions when the apprentice creates an
architecture, so on this level it is expected to have issues with it. It's not
required to come up with a well designed architecture rather just start to think
about it.

While the apprentice thinks about the architectural issues, they practice all
the basic building blocks that was presented during the foundation phase.

We only provide high level descriptions of the features, and the apprentice has
to come up with the explicit instructions for the implementation.

This is one of the first bigger projects that the apprentice has to deliver. We
have introduced the kanban method in previous projects. This is a great
opportunity to practice kanban on a bigger scale. Please follow the principles,
and show your work to a mentor for review, before you would have more than 2
tasks in the doing column.


## Workshop: Plan your work

### 0. Fork this repository (under your user)

### 1. Clone the repository to your computer

### 2. Go through the technical details

#### How to launch the program

- We will use our FoxDraw.cs class in the following examples

  - But you can write your own until the end of the week! :)

- When reading through the specification and the stories
  again keep this in mind.

- Here's an example, it contains

  - a big drawable canvas
  - and handling pressing keys, for moving your hero around
  - be aware that these are just all the needed concepts put in one place
  - you can separate anything anyhow  
  
  ```csharp

  public partial class MainWindow : Window
  {
      public MainWindow()
      {
          InitializeComponent();
          var canvas = this.Get<Canvas>("canvas");
          var foxDraw = new FoxDraw(canvas);
          
          this.KeyUp += MainWindow_KeyUp;
      }

      private void MainWindow_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
      {
          Console.WriteLine(e.Key);
      }
  }
  ```

  - You can add images with Avalonia like this:

    ```csharp
    var image = new Avalonia.Controls.Image();
    image.Source = new Avalonia.Media.Imaging.Bitmap(@"../floor.png");
    foxDraw.AddImage(image, 100, 200);              // x and y are coordinates for image
    ```

### 3. Form groups and plan your application together

Plan your architecture. In your architecture you 
should consider the following components:

- Models

  - GameObject

    - Character

      - Monster
      - Hero
      - types

    - Area

      - Tile

        - EmptyTile
        - NotEmptyTile

- GameLogic

  - current hero
  - current area
  
- Main

  - handling events
  - current game

### 4. Follow the [project stories](https://github.com/green-fox-academy/Wanderer-cs-Latest/blob/master/ProjectStories.md)

### 5. Follow the [game logic](https://github.com/green-fox-academy/Wanderer-cs-Latest/blob/master/GameLogic.md)
     
### 6. Think about task breakdown in Kanban together

Now that you see the big picture, **go through the stories together**
and think about how to break them down into tasks:

- To classes
- To methods
- To data and actions
- Extend the story cards with some of these points as a reminder

### 7. Start working on your first task!
