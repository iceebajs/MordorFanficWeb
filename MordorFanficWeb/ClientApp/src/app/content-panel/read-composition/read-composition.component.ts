import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Composition } from '../../shared/interfaces/composition/composition.interface';
import { CompositionService } from '../../shared/services/composition.service';
import { AuthorizationService } from '../../shared/services/authorization.service';
import { AccountService } from '../../shared/services/account.service';
import { ChapterService } from '../../shared/services/chapter.service';
import { take } from 'rxjs/operators';
import { Chapter } from '../../shared/interfaces/chapter/chapter.interface';
import { Tag } from '../../shared/interfaces/tags/tag.interface';
import { Rating } from '../../shared/interfaces/composition/rating.interface';
import { Like } from '../../shared/interfaces/chapter/like.interface';
import { UserCommentary } from '../../shared/interfaces/composition/comment.interface';
import { User } from '../../shared/interfaces/user.interface';

@Component({
  selector: 'app-read-composition',
  templateUrl: './read-composition.component.html',
  styleUrls: ['./read-composition.component.css'],
  providers: [CompositionService, AccountService, ChapterService]
})
export class ReadCompositionComponent implements OnInit {

  constructor(private route: ActivatedRoute, private compositionService: CompositionService,
    private authService: AuthorizationService, private accountService: AccountService,
    private chapterService: ChapterService) { }

  ngOnInit(): void {
    this.accountService.getUserAccountId(localStorage.getItem('id')).pipe(take(1)).subscribe((response) => {
      this.currentAccountId = response;
    });
    this.accountService.getUserById(localStorage.getItem('id')).pipe(take(1)).subscribe((response: User) => this.userName = response.userName);
    this.compositionId = Number(this.route.snapshot.paramMap.get('id'));
    this.compositionService.getAllTags()
      .pipe(take(1))
      .subscribe((response: Tag[]) => { this.tagsList = response; this.getComposition(); });
    this.isLoggedIn = this.authService.isSignedIn();
    this.setLikeButtonStatus();
  }

  getComposition() {
    this.compositionService.getCompositionById(this.compositionId)
      .pipe(take(1))
      .subscribe((response: Composition) => {
        this.currentComposition = response;
        this.dataLoaded = Promise.resolve(true);
        this.calculateRatings(response.compositionRatings);
        this.setCommentsAndRatings();
      },
        () => { this.hasError = true; this.errorMessage = 'Composition not found.' });
  }

  setCommentsAndRatings() {
    this.checkVote();
    this.commentsCount = this.currentComposition.compositionComments.length;
    this.switchComments(1);
    this.currentComposition.chapters.length < 1 ? this.setChapterError() : this.setChapter();
  }

  userName: string;
  currentAccountId: number;
  isLoggedIn: boolean = false;
  userRate = 0;

  hasError: boolean = false;
  errorMessage: string = '';
  dataLoaded: Promise<boolean>;
  page: number = 1;

  compositionId: number;
  currentComposition: Composition;
  currentChapter: Chapter;
  chapterCount: number;
  tagsList: Tag[];
  compositionRating: number = 0;
  sortedChapters: Chapter[] = [];

  private sotrChaptersByNumeration() {
    this.currentComposition.chapters.sort(function (a, b) {
      return a.chapterNumber - b.chapterNumber;
    });
  }

  private setChapter() {
    this.sotrChaptersByNumeration();
    this.currentChapter = this.currentComposition.chapters[0];
    this.chapterCount = this.currentComposition.chapters.length;
  }

  private setChapterError() {
    this.hasError = true;
    this.errorMessage = 'Author of this composition still not created any chapters.';
  }

  isRead: boolean = false;
  readComposition() {
    this.isRead = true;
    this.checkLike();
  }

  searchByTag(index) {
    index = index + 1;
    console.log(index);
  }

  calculateRatings(ratings: Rating[]) {
    let summary = 0;
    if (ratings.length > 0) {
      ratings.forEach(rating => {
        summary = summary + rating.rating;
      });
      this.compositionRating = (summary / ratings.length);
    }
    else
      this.compositionRating = summary;
  }

  switchChapter(p: number) {
    this.currentChapter = this.currentComposition.chapters[p - 1];
    this.checkLike();
  }

  voted: boolean = false;
  setRating() {
    this.voted = true;
    setTimeout(() => {
      const rating: Rating = {
        accountId: this.currentAccountId,
        rating: this.userRate,
        compositionId: this.currentComposition.compositionId
      } as Rating;
      this.compositionService.addRating(rating).pipe(take(1)).subscribe();
    }, 1000);
  }

  checkVote() {
    for (let v of this.currentComposition.compositionRatings) {
      if (v.accountId == this.currentAccountId) {
        this.voted = true;
        this.userRate = v.rating;
      }
    }
  }

  likeButtonDisabled: boolean = true;
  isAlreadyLiked: boolean = false;
  currentUserLiked: boolean = false;
  likesCount: number = 0;

  setLikeButtonStatus() {
    this.likeButtonDisabled = this.isLoggedIn;
  }

  likeChapter() {
    this.isAlreadyLiked = true;
    if (!this.currentUserLiked) {
      this.likesCount += 1;
      const like: Like = {
        accountId: this.currentAccountId,
        chapterId: this.currentChapter.chapterId
      } as Like;
      this.currentUserLiked = true;
      this.currentComposition.chapters[this.page - 1].chapterLikes.push(like);
      this.chapterService.addLike(like).pipe(take(1)).subscribe();
    }
  }

  checkLike() {
    this.likesCount = this.currentChapter.chapterLikes.length;
    this.currentUserLiked = false;
    this.isAlreadyLiked = false;
    if (this.likesCount > 0)
      for (let l of this.currentChapter.chapterLikes) {
        if (l.accountId == this.currentAccountId) {
          this.currentUserLiked = true;
          this.isAlreadyLiked = true;
        }
      }
  }

  //comments
  currentUserComment: string = '';
  submitComment() {
    if (this.currentUserComment.length > 0) {
      const comment: UserCommentary = {
        userName: this.userName,
        commentContext: this.currentUserComment,
        compositionId: this.currentComposition.compositionId
      } as UserCommentary;
      this.compositionService.addComment(comment).pipe(take(1)).subscribe(() => this.currentUserComment = '');
    }
  }

  commentsPage = 1;
  commentsCount: number;
  currentComments: UserCommentary[] = [];
  switchComments(p: number) {
    let i = 0;
    this.currentComments.length = 0;
    while (i < 6) {
      if (this.currentComposition.compositionComments[i + 6 * (p - 1)])
        this.currentComments.push(this.currentComposition.compositionComments[i + 6 * (p - 1)]);
      i++;
    }

  }
}

