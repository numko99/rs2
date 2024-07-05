import 'package:iter_mobile/models/image_model.dart';
import 'package:json_annotation/json_annotation.dart';

part 'arrangement_search_response.g.dart';

@JsonSerializable()
class ArrangementSearchResponse {
  final String id;
  final String name;
  final String agencyName;
  final double agencyRating;
  final DateTime startDate;
  DateTime? endDate;
  final int? arrangementStatusId;
  final String? arrangementStatusName;
  final ImageModel mainImage;
  final bool isReserved;
  final double? minPrice;

  ArrangementSearchResponse({
    required this.id,
    required this.name,
    required this.agencyName,
    required this.agencyRating,
    required this.startDate,
    this.endDate,
    required this.arrangementStatusId,
    required this.arrangementStatusName,
    required this.mainImage,
    required this.isReserved,
    this.minPrice,
  });

  factory ArrangementSearchResponse.fromJson(Map<String, dynamic> json) =>
      _$ArrangementSearchResponseFromJson(json);

  Map<String, dynamic> toJson() => _$ArrangementSearchResponseToJson(this);
}
