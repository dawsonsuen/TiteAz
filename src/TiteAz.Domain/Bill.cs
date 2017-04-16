using System;
using System.Collections.Generic;
using System.Linq;
using NEvilES;

namespace TiteAz.Domain
{
    public abstract class Bill
    {

        public class Create : ICommand
        {
            public Create() { }
            public Create(Guid streamId, string name, string description, decimal amount)
            {
                StreamId = streamId;
                Name = name;
                Description = description;
                Amount = amount;
            }

            public Guid StreamId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
        }


        public class Created : Create, IEvent
        {
            public Created() { }
            public Created(Guid guid, string name, string description, decimal amount) : base(guid, name, description, amount)
            {
            }
        }

        public class AddComment : ICommand
        {
            public AddComment() { }

            public AddComment(Guid streamId, Guid commentId, string comment)
            {
                this.StreamId = streamId;
                this.CommentId = commentId;
                this.Comment = comment;
            }

            public Guid StreamId { get; set; }
            public Guid CommentId { get; set; }
            public string Comment { get; set; }
        }

        public class CommentAdded : AddComment, IEvent
        {
            public CommentAdded() { }
            public CommentAdded(Guid streamId, Guid commentId, string comment) : base(streamId, commentId, comment)
            {
            }
        }


        public class EditComment : ICommand
        {
            public EditComment() { }
            public EditComment(Guid streamId, Guid commentId, string updatedComment)
            {
                this.StreamId = streamId;
                this.CommentId = commentId;
                this.Comment = updatedComment;
            }

            public Guid StreamId { get; set; }
            public Guid CommentId { get; set; }
            public string Comment { get; set; }
        }

        public class CommentEditted : EditComment, IEvent
        {
            public CommentEditted() { }
            public CommentEditted(Guid streamId, Guid commentId, string updatedComment) : base(streamId, commentId, updatedComment)
            {
            }
        }

        public class RemoveComment : ICommand
        {
            public RemoveComment() { }
            public RemoveComment(Guid streamId, Guid commentId)
            {
                this.StreamId = streamId;
                this.CommentId = commentId;
            }
            public Guid StreamId { get; set; }
            public Guid CommentId { get; set; }
        }

        public class CommentRemoved : RemoveComment, IEvent
        {
            public CommentRemoved() { }
            public CommentRemoved(Guid streamId, Guid commentId) : base(streamId, commentId)
            {
            }
        }

        public class Aggregate : AggregateBase,
             IHandleAggregateCommand<Create>,
             //these will have to be non stateless
             IHandleAggregateCommand<AddComment>,
             //IHandleStatelessEvent<CommentAdded>,
             IHandleAggregateCommand<EditComment>,
             IHandleAggregateCommand<RemoveComment>
        {
            public void Handle(AddComment command)
            {
                Raise<CommentAdded>(command);
            }

            public void Handle(EditComment command)
            {
                Raise<CommentEditted>(command);
            }

            public void Handle(RemoveComment command)
            {
                Raise<CommentRemoved>(command);
            }

            public void Handle(Create command)
            {
                Raise<Created>(command);
            }

            /* ----------------------------------------------------- */

            public void Apply(Created e)
            {

            }
            public void Apply(CommentAdded e)
            {
                Comments.Add(new Comment(e.CommentId, e.Comment));
            }

            public void Apply(CommentEditted e)
            {
                var comment = Comments.Where(x => x.Id == e.CommentId).SingleOrDefault();

                if (comment == null)
                {
                    throw new DomainAggregateException(this, $"Comment of id: {comment.Id} does not exist on aggregate");
                }

                comment.Text = e.Comment;
            }

            public void Apply(CommentRemoved e)
            {
                var comment = Comments.Where(x => x.Id == e.CommentId).SingleOrDefault();

                if (comment == null)
                {
                    throw new DomainAggregateException(this, $"Comment of id: {comment.Id} does not exist on aggregate");
                }

                Comments.Remove(comment);
            }

            /* ----------------------------------------------------- */
            private List<Comment> Comments { get; set; } = new List<Comment>();

        }
    }
    public class Comment
    {
        public Comment(Guid id, string comment)
        {
            this.Id = id;
            this.Text = comment;
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}