# Book Manager Project
Welcome to **Book Manager**, a console-based application for managing your personal library and _To Be Read_ (TBR) list! This project allows you to organize your books, track reading goals, and manage metadata like tags—all from an intuitive console interface.

## How to Use
- Run the application from your console or an IDE such as Visual Studio.
- Select an item from the Main Menu Options:
  - View Shelf and TBR: Display all books currently stored.
  - Add Book: Add new books to your shelf or TBR list.
  - Organize Books: Move books between lists or remove them.
  - Manage Tags: Add or remove tags for any book.
  - Exit: Save your changes and close the application.
- When adding a book, you will be prompted to provide a Title, Author, Price and Tags.
- Once they have been input, books are categorized as either "Books on Shelf" or "Books in TBR" and then saved to a file.

## Features
- Organize Your Library:
  - Add books to a personal bookshelf or a TBR list.
  - Move books between your shelf and TBR list.
  - Remove books from either list.
- Metadata Management:
  - Add or remove tags to categorize books (e.g., "Fantasy", "Non-Fiction").
  - View all books along with their tags, author, and price details.
- Persistent Storage:
  - Save your library and TBR list to a file to ensure data persists across sessions.
  - Load saved data when starting the program.
- Error Handling & Input Validation:
  - Validates user inputs for book attributes like title, author, and price.
  - Ensures duplicate entries are avoided and invalid data is handled gracefully.

## Technologies Used
- Language: C#
- Framework: .NET Core
- IDE: Visual Studio

## Future Improvements
- Sorting: Add sorting options for viewing books (e.g., by title, author, or price).
- Advanced Tagging: Include tag-based search and filtering.
- Graphical Interface: Develop a user-friendly GUI for easier interaction.
- Integration: Sync with online libraries like Goodreads
