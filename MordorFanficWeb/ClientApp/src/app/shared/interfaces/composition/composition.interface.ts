import { Chapter } from './../chapter/chapter.interface';
import { CompositionTag } from './../composition-tags/composition-tag.interface'
import { Rating } from './rating.interface';
import { UserCommentary } from './comment.interface';

export interface Composition{
  compositionId: number;
  title: string;
  previewContext: string;
  genre: string;
  userId: number;
  chapters?: Chapter[];
  compositionTags: CompositionTag[];
  compositionRatings?: Rating[];
  compositionComments?: UserCommentary[];
}
