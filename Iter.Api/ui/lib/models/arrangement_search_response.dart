import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/image_model.dart';

part 'arrangement_search_response.g.dart';

@JsonSerializable()
class ArrangementSearchResponse {
  final String id;
  final String name;
  final String agencyName;
  final double agencyRating;
  final DateTime startDate;
  DateTime? endDate;
  int? arrangementStatusId;
  String? arrangementStatusName;
  ImageModel? mainImage;
  bool? isReserved;
  double? rating;

  ArrangementSearchResponse({
    required this.id,
    required this.name,
    required this.agencyName,
    required this.agencyRating,
    required this.startDate,
    this.endDate,
    this.arrangementStatusId,
    this.arrangementStatusName,
    this.mainImage,
    this.isReserved,
    this.rating
  });

  factory ArrangementSearchResponse.fromJson(Map<String, dynamic> json) =>
      _$ArrangementSearchResponseFromJson(json);

  Map<String, dynamic> toJson() => _$ArrangementSearchResponseToJson(this);
}
