import { Like } from './like.interface';

export interface Chapter {
  chapterId: number;
  chapterNumber: number;
  chapterTitle: string;
  context: string;
  imgSource: string;
  compositionId: number;
  chapterLikes: Like[];
}
