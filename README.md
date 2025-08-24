# SafeGeneral-ABB

## Table of Contents

- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Usage](#usage)

## Project Structure

The project is structured as follows:

```
SafeGeneral-ABB/
├── .gitattributes
├── .gitignore
├── .idea/
├── ABB.Api/
├── ABB.Application/
├── ABB.Domain/
├── ABB.Infrastructure/
├── ABB.Web/
├── ABB.sln
├── ABB.sln.DotSettings.user
├── Include/
└── global.json
```

- `ABB.Api`: Contains the API layer of the application.
- `ABB.Application`: Contains the application logic.
- `ABB.Domain`: Contains the domain models and entities.
- `ABB.Infrastructure`: Contains the infrastructure-related code, such as database connections and external services.
- `ABB.Web`: Contains the web application.
- `ABB.sln`: The solution file for the project.

## Prerequisites
- Net Core 3.1

## Usage

This section provides examples of how to use MediatR, AutoMapper, and FluentValidation within the project.

### MediatR

MediatR is used for implementing the Mediator pattern, enabling decoupled communication between different parts of the application.

#### Example: Command Handling

Let's say you have a command to create a new user.

1.  **Define the Command:**

    ```csharp
    public class CreateUserCommand : IRequest<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
    ```

2.  **Create the Handler:**

    ```csharp
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email
            };

            await _userRepository.AddAsync(user);
            return user;
        }
    }
    ```

3.  **Dispatch the Command:**

    ```csharp
    public async Task<IActionResult> CreateUser(string username, string email, IMediator mediator)
    {
        var command = new CreateUserCommand { Username = username, Email = email };
        var user = await mediator.Send(command);
        return Ok(user);
    }
    ```

#### Example: Query Handling

Let's say you have a query to get a user by ID.

1.  **Define the Query:**

    ```csharp
    public class GetUserQuery : IRequest<User>
    {
        public int Id { get; set; }
    }
    ```

2.  **Create the Handler:**

    ```csharp
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            return user;
        }
    }
    ```

3.  **Dispatch the Query:**

    ```csharp
    public async Task<IActionResult> GetUser(int id, IMediator mediator)
    {
        var query = new GetUserQuery { Id = id };
        var user = await mediator.Send(query);
        return Ok(user);
    }
    ```

### AutoMapper

AutoMapper is used for mapping objects from one type to another. In this project, AutoMapper configurations are defined directly within the ViewModels, DTOs, Commands, or Queries.

#### Example: Mapping a User Entity to a UserDto

1.  **Define the DTO (with Mapping Configuration):**

    ```csharp
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>();
        }
    }
    ```

    **Note:** The `Mapping` method is an example. You might use a different approach to configure the mapping within your DTO/ViewModel. The key is that the mapping logic resides within the class itself.

2.  **Use the Mapper:**

    ```csharp
    public async Task<IActionResult> GetUser(int id, IMediator mediator, IMapper mapper)
    {
        var query = new GetUserQuery { Id = id };
        var user = await mediator.Send(query);
        var userDto = mapper.Map<UserDto>(user);
        return Ok(userDto);
    }
    ```

### FluentValidation

FluentValidation is used for request validation, and the validation logic is applied directly within the controller actions.

#### Example: Integrating FluentValidation in Controller

1.  **Define a Validator:**

    ```csharp
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress();
        }
    }
    ```
2. **Use on Controller:**

   ```csharp
   public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            try
            {
                var command = Mapper.Map<CreateUserCommand>(model);
                command.DatabaseName = Request.Cookies["DatabaseValue"];
                await Mediator.Send(command);
                return Json(new { Result = "OK", Message = Constant.DataDisimpan});
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelErrors(ex);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
            return PartialView(model);
        }
   ```
