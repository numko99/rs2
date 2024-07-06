import 'package:json_annotation/json_annotation.dart';
import 'package:iter_mobile/models/agency.dart';
import 'package:iter_mobile/models/arrangement_price.dart';
import 'package:iter_mobile/models/destination.dart';
import 'package:iter_mobile/models/image_model.dart';

part 'arrangment.g.dart';

@JsonSerializable()
class Arrangement {
  final String id;
  final String name;
  final Agency agency;
  final String description;
  final String shortDescription;
  final DateTime startDate;
  final DateTime? endDate;
  final List<ArrangementPrice> prices;
  final List<ImageModel> images;
  final List<Destination> destinations;
  final int? arrangementStatusId;
  final String? arrangementStatusName;
  final DateTime? modifiedAt;

  Arrangement({
    required this.id,
    required this.name,
    required this.agency,
    required this.description,
    required this.shortDescription,
    required this.startDate,
    required this.endDate,
    required this.prices,
    required this.images,
    required this.destinations,
    required this.arrangementStatusId,
    required this.arrangementStatusName,
    required this.modifiedAt,
  });
  factory Arrangement.fromJson(Map<String, dynamic> json) =>
      _$ArrangementFromJson(json);

  Map<String, dynamic> toJson() => _$ArrangementToJson(this);
}
