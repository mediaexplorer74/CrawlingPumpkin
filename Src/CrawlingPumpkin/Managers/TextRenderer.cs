
// Type: ScreamJam24.Managers.TextRenderer
// Assembly: Crawling Pumpkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6FFA2B6B-8BEA-4A04-9817-EC2892B8097F


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace ScreamJam24.Managers
{
  internal class TextRenderer
  {
    private Texture2D _texture;
    private Queue<Rectangle> _drawQ;
    private Dictionary<char, Rectangle> _storedPositions;
    private int _fontWidth;
    private int _rowCount;
    private int _columnCount;
    private int _letterSpacing;
    private int _fontScale;
    private int _asciiStartAt;
    private int _asciiEndAt;

    public TextRenderer(Texture2D LettersSheet)
    {
      this._texture = LettersSheet;
      this._drawQ = new Queue<Rectangle>();
      this._storedPositions = new Dictionary<char, Rectangle>(this._asciiEndAt
          - this._asciiStartAt + 1);
      this._asciiStartAt = 32;
      this._asciiEndAt = 126;
      this.SetFontWidthInPixels(8);
      this.SetFontScale(2);
    }

    public void SetFontWidthInPixels(int width)
    {
      this._fontWidth = width;
      if (this._texture.Width % width != 0 || this._texture.Height % width != 0)
        throw new Exception("Provided character width " + width.ToString() 
            + ", is not compatible with provided character sheet " 
            + this._texture.Width.ToString() + "x" + this._texture.Height.ToString() + ".");
      this._rowCount = this._texture.Width / width;
      this._columnCount = this._texture.Height / width;
      this._storedPositions.Clear();
    }

    public void SetAsciiRange(int startAt, int endAt)
    {
      this._asciiStartAt = this._asciiStartAt < this._asciiEndAt
                ? startAt
                : throw new Exception("Range for allowed ascii range cannot be negative or 0.");
      this._asciiEndAt = endAt;
      this._storedPositions.Clear();
    }

    public void CondenseLetterSpacing(int width)
    {
      this._letterSpacing = width <= this._fontWidth / 2
                ? width : throw new Exception("Letter spacing cannot exceed half " +
                "of the letter width, if this is intended, use SetLetterSpacingUnsafe()");
    }

    public void CondenseLetterSpacingUnsafe(int width) => this._letterSpacing = width;

    public void ExpandLetterSpacing(int width) => this._letterSpacing = -width;

    public Rectangle MeasureString(string text)
    {
      return new Rectangle(0, 0, text.Length * (
          this._fontWidth * this._fontScale - this._letterSpacing * this._fontScale)
          + this._letterSpacing * this._fontScale, this._fontWidth * this._fontScale);
    }

    public void SetFontScale(int scale)
    {
      this._fontScale = scale > 0 
                ? scale 
                : throw new Exception("Font scale cannot be lower than 1.");
    }

    public void DrawString(SpriteBatch _spriteBatch, string text, Vector2 position)
    {
      this.ParseString(text);
      while (this._drawQ.Count > 0)
      {
        _spriteBatch.Draw(this._texture, new Rectangle((int) position.X,
            (int) position.Y, this._fontWidth * this._fontScale,
            this._fontWidth * this._fontScale), new Rectangle?(this._drawQ.Peek()), Color.White);
        this._drawQ.Dequeue();
        position.X += (float) (this._fontWidth * this._fontScale -
                    this._letterSpacing * this._fontScale);
      }
    }

    public void DrawStringWrapAround(
      SpriteBatch _spriteBatch,
      string text,
      Vector2 position,
      int maxWidth)
    {
      if (this.MeasureString(text).Width > maxWidth)
      {
        this.StoreString(" ");
        this.ParseString(text);
        int num1 = 0;
        int x = (int) position.X;
        int num2 = this._fontWidth * this._fontScale - this._letterSpacing * this._fontScale;
        Queue<Rectangle> rectangleQueue = new Queue<Rectangle>();
        while (this._drawQ.Count > 0)
        {
          while (this._drawQ.Count > 0 && this._drawQ.Peek() != this._storedPositions[' '])
            rectangleQueue.Enqueue(this._drawQ.Dequeue());
          int num3 = num2 * rectangleQueue.Count;
          try
          {
            if (num3 > maxWidth)
              throw new Exception("The string contains a word " +
                  "that alone exceeds the given max width. " +
                  "Use DrawStringWrapAroundHyphenate() instead.");
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex.Message);
            break;
          }
          if (num1 + num3 <= maxWidth)
          {
            while (rectangleQueue.Count > 0)
            {
              _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                  (int) position.Y, this._fontWidth * this._fontScale,
                  this._fontWidth * this._fontScale), 
                  new Rectangle?(rectangleQueue.Dequeue()), Color.White);
              position.X += (float) num2;
            }
          }
          else
          {
            position.Y += (float) (this._fontWidth * this._fontScale);
            position.X = (float) x;
            num1 = 0;
            while (rectangleQueue.Count > 0)
            {
              _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                  (int) position.Y, this._fontWidth * this._fontScale,
                  this._fontWidth * this._fontScale), 
                  new Rectangle?(rectangleQueue.Dequeue()), Color.White);
              position.X += (float) num2;
            }
          }
          num1 += num3;
          if (this._drawQ.Count > 0)
          {
            _spriteBatch.Draw(this._texture, new Rectangle((int) position.X,
                (int) position.Y, this._fontWidth * this._fontScale, 
                this._fontWidth * this._fontScale), 
                new Rectangle?(this._drawQ.Dequeue()), Color.White);
            position.X += (float) num2;
            num1 += num2;
          }
        }
      }
      else
        this.DrawString(_spriteBatch, text, position);
    }

    public void DrawStringWrapAroundCentered(
      SpriteBatch _spriteBatch,
      string text,
      Vector2 position,
      int maxWidth)
    {
      this.StoreString(" ");
      this.ParseString(text);
      int num1 = 0;
      int x = (int) position.X;
      int num2 = this._fontWidth * this._fontScale - this._letterSpacing * this._fontScale;
      Queue<Rectangle> rectangleQueue1 = new Queue<Rectangle>();
      Queue<Rectangle> rectangleQueue2 = new Queue<Rectangle>();
      while (this._drawQ.Count > 0)
      {
        while (this._drawQ.Count > 0 && this._drawQ.Peek() != this._storedPositions[' '])
          rectangleQueue1.Enqueue(this._drawQ.Dequeue());
        int num3 = num2 * rectangleQueue1.Count;
        try
        {
            if (num3 > maxWidth)
            {
                throw new Exception("The string contains a word " +
                    "that alone exceeds the given max width. " +
                    "Use DrawStringWrapAroundHyphenate() instead.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            break;
        }
        if (num1 + num3 <= maxWidth)
        {
          while (rectangleQueue1.Count > 0)
            rectangleQueue2.Enqueue(rectangleQueue1.Dequeue());
          if (this._drawQ.Count > 0 && this._drawQ.Peek() == this._storedPositions[' '])
          {
            rectangleQueue2.Enqueue(this._drawQ.Dequeue());
            num1 += num2;
          }
          num1 += num3;
        }
        else
        {
          position.X += (float) ((maxWidth - num1 + num2 / 2) / 2);
          while (rectangleQueue2.Count > 0)
          {
            _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                (int) position.Y, this._fontWidth * this._fontScale, 
                this._fontWidth * this._fontScale),
                new Rectangle?(rectangleQueue2.Dequeue()), Color.White);
            position.X += (float) num2;
          }
          rectangleQueue2.Clear();
          num1 = 0;
          position.X = (float) x;
          position.Y += (float) (this._fontWidth * this._fontScale);
        }
        if (this._drawQ.Count == 0)
        {
          if (rectangleQueue1.Count > 0)
          {
            num1 = num2 * rectangleQueue1.Count;
            position.X = (float) (x + (maxWidth - num1) / 2);
            while (rectangleQueue1.Count > 0)
            {
              _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                  (int) position.Y, this._fontWidth * this._fontScale,
                  this._fontWidth * this._fontScale), 
                  new Rectangle?(rectangleQueue1.Dequeue()), Color.White);
              position.X += (float) num2;
            }
          }
          else if (rectangleQueue2.Count > 0)
          {
            num1 = num2 * rectangleQueue2.Count;
            position.X = (float) (x + (maxWidth - num1) / 2);
            while (rectangleQueue2.Count > 0)
            {
              _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                  (int) position.Y, this._fontWidth * this._fontScale, 
                  this._fontWidth * this._fontScale), 
                  new Rectangle?(rectangleQueue2.Dequeue()), Color.White);
              position.X += (float) num2;
            }
          }
        }
      }
    }

    public void DrawStringWrapAroundHyphenate(
      SpriteBatch _spriteBatch,
      string text,
      Vector2 position,
      int maxWidth,
      string punctuations = "")
    {
      if (this.MeasureString(text).Width > maxWidth)
      {
        this.StoreString(" -");
        try
        {
          if (this._storedPositions.TryGetValue('-', out Rectangle _))
          {
            if (this._storedPositions.TryGetValue(' ', out Rectangle _))
              goto label_6;
          }
          throw new Exception("Cannot hyphenate since atlas does not contain hyphen and/or space.");
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
          return;
        }
label_6:
        List<Rectangle> rectangleList = this.StoreString(punctuations);
        try
        {
          if (rectangleList.Count != punctuations.Length)
            throw new Exception("Some of the puncuations provided does not exist in the atlas. " +
                "Hyphenation might not work as intended.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        this.ParseString(text);
        int num1 = 0;
        int x = (int) position.X;
        bool flag = false;
        while (this._drawQ.Count > 0)
        {
          int num2 = this._fontWidth * this._fontScale - this._letterSpacing * this._fontScale;
          if (num1 == 0 && this._drawQ.Peek() == this._storedPositions[' '])
          {
            this._drawQ.Dequeue();
          }
          else
          {
            num1 += num2;
            if (num1 >= maxWidth - num2)
            {
              if (this._drawQ.Peek() != this._storedPositions[' '])
              {
                if (!flag)
                {
                  if (!rectangleList.Contains(this._drawQ.Peek()))
                  {
                    _spriteBatch.Draw(this._texture, new Rectangle((int) position.X,
                        (int) position.Y, this._fontWidth * this._fontScale, 
                        this._fontWidth * this._fontScale), 
                        new Rectangle?(this._storedPositions['-']), Color.White);
                  }
                  else
                  {
                    _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                        (int) position.Y, this._fontWidth * this._fontScale, 
                        this._fontWidth * this._fontScale), 
                        new Rectangle?(this._drawQ.Peek()), Color.White);
                    position.Y += (float) (this._fontWidth * this._fontScale);
                    position.X = (float) x;
                    num1 = 0;
                    this._drawQ.Dequeue();
                    continue;
                  }
                }
                position.Y += (float) (this._fontWidth * this._fontScale);
                position.X = (float) x;
                num1 = num2;
              }
              else
              {
                position.Y += (float) (this._fontWidth * this._fontScale);
                position.X = (float) x;
                num1 = 0;
                this._drawQ.Dequeue();
                continue;
              }
            }
            _spriteBatch.Draw(this._texture, new Rectangle((int) position.X, 
                (int) position.Y, this._fontWidth * this._fontScale, 
                this._fontWidth * this._fontScale), new Rectangle?(this._drawQ.Peek()), Color.White);
            position.X += (float) num2;
            flag = this._drawQ.Peek() == this._storedPositions[' '];
            this._drawQ.Dequeue();
          }
        }
      }
      else
        this.DrawString(_spriteBatch, text, position);
    }

    private void ParseString(string text)
    {
      foreach (char key in text)
      {
        Rectangle rectangle;
        if (this._storedPositions.TryGetValue(key, out rectangle))
        {
          this._drawQ.Enqueue(rectangle);
        }
        else
        {
          int charPosition = (int) key - this._asciiStartAt;
          try
          {
            if (charPosition >= 0)
            {
              if (charPosition <= 95)
                goto label_9;
            }
            string[] strArray = new string[7];
            strArray[0] = "Only ascii from ";
            strArray[1] = this._asciiStartAt.ToString();
            char ch = (char) this._asciiStartAt;
            strArray[2] = ch.ToString();
            strArray[3] = " to ";
            strArray[4] = this._asciiEndAt.ToString();
            ch = (char) this._asciiEndAt;
            strArray[5] = ch.ToString();
            strArray[6] = " allowed.";
            throw new Exception(string.Concat(strArray));
          }
          catch (Exception ex)
          {
             Debug.WriteLine("[ex] TextRenderer error: " + ex.Message);
                        charPosition = 63 - this._asciiStartAt;
          }
label_9:
          Rectangle targetRectangle = this.GetTargetRectangle(charPosition);
          this._storedPositions.Add(key, targetRectangle);
          this._drawQ.Enqueue(targetRectangle);
        }
      }
    }

    private List<Rectangle> StoreString(string text)
    {
      List<Rectangle> rectangleList = new List<Rectangle>();
      foreach (char key in text)
      {
        Rectangle rectangle;
        if (!this._storedPositions.TryGetValue(key, out rectangle))
        {
          int charPosition = (int) key - this._asciiStartAt;
          try
          {
            if (charPosition >= 0)
            {
              if (charPosition <= 95)
                goto label_7;
            }
            throw new Exception("Only ascii from " + this._asciiStartAt.ToString() 
                + ((char) this._asciiStartAt).ToString() + " to "
                + this._asciiEndAt.ToString() + ((char) this._asciiEndAt).ToString()
                + " allowed.");
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex.Message);
            charPosition = 63 - this._asciiStartAt;
          }
label_7:
          Rectangle targetRectangle = this.GetTargetRectangle(charPosition);
          this._storedPositions.Add(key, targetRectangle);
          rectangleList.Add(targetRectangle);
        }
        else if (!rectangleList.Contains(rectangle))
          rectangleList.Add(rectangle);
      }
      return rectangleList;
    }

    private Rectangle GetTargetRectangle(int charPosition)
    {
      int num1 = charPosition % this._rowCount;
      int num2 = charPosition / this._rowCount;
      int fontWidth = this._fontWidth;
      return new Rectangle(num1 * fontWidth, num2 * this._fontWidth, this._fontWidth, this._fontWidth);
    }
  }
}
